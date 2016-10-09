using System;
using System.Collections.Generic;

using NLog;
using Akka.Actor;
using Gomoku.Common;
using System.Threading.Tasks;

//
// Acknowledge:
// The (simple) "AI" engine was implemented using the algorithm from:
// http://www.cise.ufl.edu/~cop4600/cgi-bin/lxr/http/source.cgi/commands/simple/gomoku.c
//

namespace Gomoku.Actors
{
    /// <summary>
    /// 
    /// </summary>
    internal class GameActor : ReceiveActor
    {
        /// <summary>
        /// 
        /// </summary>
        internal class Board : ICloneable
        {
            List<char> cells;

            public int Size { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="size"></param>
            public Board(int size)
            {
                this.Size = size;
                this.cells = new List<char>(new char[size * size]);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="row"></param>
            /// <param name="col"></param>
            /// <param name="color"></param>
            public void Set(int row, int col, char color)
            {
                this.cells[row * this.Size + col] = color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="row"></param>
            /// <param name="col"></param>
            /// <returns></returns>
            public char Get(int row, int col)
            {
                return this.cells[row * this.Size + col];
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="row"></param>
            /// <param name="col"></param>
            public bool IsEmpty(int row, int col)
            {
                return this.cells[row * this.Size + col] == '\0';
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            object ICloneable.Clone()
            {
                return new Board(this.Size);
            }
        }

        #region States

        readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The GUID of the current game.
        /// </summary>
        readonly Guid guid;

        /// <summary>
        /// The game server actor.
        /// </summary>
        IActorRef gameServer;

        /// <summary>
        /// The size of the board.
        /// </summary>
        readonly int size;

        /// <summary>
        /// The 'white' stone.
        /// </summary>
        readonly Player white;

        /// <summary>
        /// The 'black' stone.
        /// </summary>
        readonly Player black;

        /// <summary>
        /// The player whose move is next.
        /// </summary>
        Player player;

        /// <summary>
        /// The board.
        /// </summary>
        Board board;

        /// <summary>
        /// The number of the moves.
        /// </summary>
        //int movesCounter;

        /// <summary>
        /// Set if one of the player won.
        /// </summary>
        bool gameWon;

        /// <summary>
        /// 
        /// </summary>
        WinningLine winningLine = WinningLine.None;

        /// <summary>
        /// The number of empty lines left.
        /// </summary>
        int totalLines;

        /// <summary>
        /// The importance of attack (1..16)
        /// </summary>
        int attackFactor;

        /// <summary>
        /// Value of having 0, 1, 2, 3, 4 or 5 pieces in line. 
        /// </summary>
        int[] weight = { 0, 0, 4, 20, 100, 500, 0 };

        /// <summary>
        /// Value of each square for each player.
        /// </summary>
        int[][][] value;

        /// <summary>
        /// 
        /// </summary>
        int[][][][] line;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameServer"></param>
        /// <param name="guid"></param>
        /// <param name="size"></param>
        /// <param name="white"></param>
        /// <param name="black"></param>
        public GameActor(IActorRef gameServer, Guid guid, int size,
            Player white, Player black)
        {
            this.gameServer = gameServer;
            this.guid = guid;
            this.size = size;

            this.white = white;
            this.black = black;

            // Black moves first
            this.player = this.black;

            this.board = new Board(this.size);
            this.attackFactor = 4;

            this.totalLines = 2 * 2 * (size * (size - 4) + (size - 4) * (size - 4));
            this.gameWon = false;

            //
            this.value = new int[size][][];
            for (int i = 0; i < size; ++i)
            {
                this.value[i] = new int[size][];

                for (int j = 0; j < size; ++j)
                {
                    this.value[i][j] = new int[2];
                }
            }

            //
            this.line = new int[4][][][];
            for (int i = 0; i < 4; ++i)
            {
                this.line[i] = new int[size][][];

                for (int j = 0; j < size; ++j)
                {
                    this.line[i][j] = new int[size][];

                    for (int k = 0; k < size; ++k)
                    {
                        this.line[i][j][k] = new int[2];
                    }
                }
            }

            //
            Receive<StartGame>(message => this.HandleStartGame(message));
            Receive<MoveMade>(message => this.HandleMoveMade(message));
            Receive<CancelGame>(message => this.HandleCancelGame(message));
            Receive<GameCancelled>(message => this.HandleGameCancelled(message));
            Receive<MakeMove>(message => this.HandleMakeMove(message));
        }

        #region Message Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStartGame(StartGame message)
        {
            var message2 = new GameStarted(this.guid);

            this.gameServer.Tell(message2);
            this.Sender.Tell(message2); // send the message straight to the client actor

            if ((this.black != null) && this.black.IsComputer())
            {
                log.Debug("Black is the AI player and will move first.");

                // 'Black' makes the first move in the middle of the board.
                var row = this.board.Size / 2;
                var column = row;

                MakeMove(row, column);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleCancelGame(CancelGame message)
        {
            this.gameServer.Tell(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleGameCancelled(GameCancelled message)
        {
            this.log.Info("Stop the game actor for game {0}", message.Guid);
            Context.Stop(this.Self);
        }

        /// <summary>
        /// A player (Human/Computer) made a move.
        /// </summary>
        /// <param name="message"></param>
        private void HandleMoveMade(MoveMade message)
        {
            this.board.Set(message.Row, message.Column, message.Player.Symbol);

            IList<Tuple<int, int>> winnigLineCoords = null;
            var status = GameStatus.Continue;
            if (this.gameWon)
            {
                status = (this.player.Color == PlayerColor.Black) ?
                    GameStatus.BlackWon : GameStatus.WhiteWon;

                winnigLineCoords = GetWinningLine(message.Row, message.Column);
            }

            if (this.totalLines <= 0)
            {
                // no more possible moves
                status = GameStatus.Draw;
            }

            // update the current player only if the game can continue
            if (status == GameStatus.Continue)
            {
                this.player = this.OpponentPlayer(message.Player);
            }

            //
            this.gameServer.Tell(
                new MoveResponse()
                {
                    Guid = this.guid,
                    Player = message.Player,
                    NextPlayer = this.player,
                    Row = message.Row,
                    Column = message.Column,
                    MoveStatus = MoveStatus.Accepted,
                    GameStatus = status,
                    WinningLine = this.winningLine,
                    WinningLineCoords = winnigLineCoords
                });

            //
            if (status != GameStatus.Continue)
            {
                // the game is over
                return;
            }

            // let the computer play its own turn
            if (this.player.IsComputer())
            {
                this.Think();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleMakeMove(MakeMove message)
        {
            if (message.Player.IsHuman())
            {
                // handles the human player move
                PlayerMoved(message.Row, message.Column, message.Player);
            }
            else
            {
                // handles the computer player move
                MakeMove(message.Row, message.Column);
                BrainMoved(message.Row, message.Column, message.Player);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private Player OpponentPlayer(Player player)
        {
            return (player.Color == PlayerColor.Black) ? this.white : this.black;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Think()
        {
            //
            this.gameServer.Tell(new StartThinking() { Guid = this.guid });

            Task.Run(async () =>
            {
                var t = await FindMove();
                return new MakeMove(this.guid, this.player, t.Item1, t.Item2);
            }).PipeTo(Self);
        }

        /// <summary>
        /// Counts the occurrences of a symbol along a given direction.
        /// While inside the bounds of the board go in forward and backward
        /// directions of the direction vector and increment the counter
        /// while still on the 'side' symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="dirRow"></param>
        /// <param name="dirCol"></param>
        /// <returns></returns>
        int CountPieces(char symbol, int row, int column, int dirRow, int dirCol)
        {
            var count = 0;
            var i = row;
            var j = column;

            // forward direction
            while ((i > -1) && (i < this.board.Size) &&
                (j > -1) && (j < this.board.Size) &&
                (this.board.Get(i, j) == symbol))
            {
                ++count;
                i += dirRow;
                j += dirCol;
            }

            // backward direction
            i = row - dirRow;
            j = column - dirCol;
            while ((i > -1) && (i < this.board.Size) &&
                (j > -1) && (j < this.board.Size) &&
                (this.board.Get(i, j) == symbol))
            {
                ++count;
                i -= dirRow;
                j -= dirCol;
            }

            return count;
        }

        /// <summary>
        /// Retrieves the coordinates of the squares which form the winning line.
        /// </summary>
        /// <returns></returns>
        IList<Tuple<int, int>> GetWinningLine(int row, int column)
        {
            log.Debug("GetWinningLine - invoked for row={0} and column={1}", row, column);

            int dirRow = 0;
            int dirColumn = 0;

            switch (this.winningLine)
            {
                case WinningLine.Horiz:
                    dirColumn = -1;
                    break;

                case WinningLine.DownLeft:
                    dirRow = 1;
                    dirColumn = -1;
                    break;

                case WinningLine.DownRight:
                    dirRow = 1;
                    dirColumn = 1;
                    break;

                case WinningLine.Vert:
                    dirRow = 1;
                    break;
            }

            //DumpBoard();

            var coords = new List<Tuple<int, int>>();

            var i = row;
            var j = column;

            // forward direction
            while ((i > -1) && (i < this.board.Size) &&
                (j > -1) && (j < this.board.Size) &&
                (this.board.Get(i, j) == this.player.Symbol))
            {
                log.Debug("GetWinningLine - adding {0}, {1}", i, j);
                coords.Add(new Tuple<int, int>(i, j));

                i += dirRow;
                j += dirColumn;
            }

            // backward direction
            i = row - dirRow;
            j = column - dirColumn;
            while ((i > -1) && (i < this.board.Size) &&
                (j > -1) && (j < this.board.Size) &&
                (this.board.Get(i, j) == this.player.Symbol))
            {
                log.Debug("GetWinningLine - adding {0}, {1}", i, j);
                coords.Add(new Tuple<int, int>(i, j));

                i -= dirRow;
                j -= dirColumn;
            }

            return coords;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        GameStatus GetWinnerForSymbol(char symbol)
        {
            return symbol == 'w' ? GameStatus.WhiteWon : GameStatus.BlackWon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="player"></param>
        void PlayerMoved(int row, int column, Player player)
        {
            if (!this.board.IsEmpty(row, column))
            {
                this.gameServer.Tell(
                    new MoveResponse()
                    {
                        Guid = this.guid,
                        Player = player,
                        NextPlayer = player,
                        MoveStatus = MoveStatus.Rejected,
                        GameStatus = GameStatus.Continue
                    });

                return;
            }

            MakeMove(row, column);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="playerColor"></param>
        void BrainMoved(int row, int column, Player player)
        {
            this.gameServer.Tell(new StopThinking() { Guid = this.guid });
        }

        /// <summary>
        /// Performs the move for 'player' and updates the variables.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        void MakeMove(int row, int column)
        {
            var opponent = OpponentPlayer(player);
            var iPlayer = (int)this.player.Color;

            this.winningLine = WinningLine.None;

            //DumpLines(iPlayer);

            /* Each square of the board is part of 20 different lines. It adds 
             * one to the number of pieces in each of these lines. Then it
             * updates the value for each of the 5 squares in each of the 20
             * lines.
             */

            // Horizontal lines, from left to right.
            for (int k = 0; k < 5; ++k)
            {
                var column1 = column - k;   // X
                var row1 = row;             // Y

                if ((0 <= column1) && (column1 < this.board.Size - 4))
                {
                    Add(ref this.line[0][row1][column1][iPlayer]);  // add one to line

                    if (this.gameWon && (this.winningLine == WinningLine.None))
                    {
                        log.Info("{0} won - winning line is Horiz", this.player);
                        this.winningLine = WinningLine.Horiz;
                    }

                    // Updates values for the 5 squares in the line.
                    for (int l = 0; l < 5; ++l)
                    {
                        if (column1 + l < this.board.Size - 4)
                        {
                            Update(this.line[0][row1][column1], this.value[row1][column1 + l], opponent);
                        }
                    }
                }
            }

            // Diagonal lines, from lower left to upper right.
            for (int k = 0; k < 5; ++k)
            {
                var column1 = column - k;   // X
                var row1 = row + k;         // Y

                if ((0 <= column1) && (column1 < this.board.Size - 4) &&
                    (0 <= row1) && (row1 < this.board.Size - 4))
                {
                    Add(ref this.line[1][row1][column1][iPlayer]);

                    if (this.gameWon && (this.winningLine == WinningLine.None))
                    {
                        log.Info("{0} won - winning line is DownLeft", this.player);
                        this.winningLine = WinningLine.DownLeft;
                    }

                    for (int l = 0; l < 5; ++l)
                    {
                        if ((row1 - l >= 0) && (column1 + l < this.board.Size - 4))
                        {
                            Update(this.line[1][row1][column1],
                                this.value[row1 - l][column1 + l], opponent);
                        }
                    }

                }
            }

            // Diagonal lines, down right to upper left.
            for (int k = 0; k < 5; ++k)
            {
                var column1 = column + k;
                var row1 = row + k;

                if ((0 <= column1) && (column1 < this.board.Size - 4) &&
                    (0 <= row1) && (row1 < this.board.Size - 4))
                {
                    Add(ref this.line[3][row1][column1][iPlayer]);

                    if (this.gameWon && (this.winningLine == WinningLine.None))
                    {
                        log.Info("{0} won - winning line is DownRight", this.player);
                        this.winningLine = WinningLine.DownRight;
                    }

                    for (int l = 0; l < 5; ++l)
                    {
                        if ((row1 - l >= 0) && (column1 - l >= 0))
                        {
                            Update(this.line[3][row1][column1],
                                this.value[row1 - l][column1 - l], opponent);
                        }
                    }

                }
            }

            // Vertical lines, from down to up
            for (int k = 0; k < 5; ++k)
            {
                var column1 = column;
                var row1 = row + k;

                if ((0 <= row1) && (row1 < this.board.Size - 4))
                {
                    Add(ref this.line[2][row1][column1][iPlayer]);

                    if (this.gameWon && (this.winningLine == WinningLine.None))
                    {
                        log.Info("{0} won - winning line is Vert", this.player);
                        this.winningLine = WinningLine.Vert;
                    }

                    for (int l = 0; l < 5; ++l)
                    {
                        if (row1 - l >= 0)
                        {
                            Update(this.line[2][row1][column1],
                                this.value[row1 - l][column1], opponent);
                        }
                    }
                }
            }

            //DumpLines(iPlayer);
            //DumpValues(iPlayer);

            //
            HandleMoveMade(
                new MoveMade(this.guid, this.player, row, column));
        }

        /// <summary>
        /// Adds one to the number of pieces in a line.
        /// </summary>
        /// <param name="num"></param>
        void Add(ref int num)
        {
            num = num + 1;

            /* If it is the first piece in the line, then the opponent cannot use
             * it any more.  
             */
            if (num == 1)
            {
                this.totalLines -= 1;
            }

            /* The game is won if there are 5 in line. */
            if (num == 5)
            {
                this.gameWon = true;
            }
        }

        /// <summary>
        /// Updates the value of a square for each player, taking into
        /// account that player has placed an extra piece in the square.
        /// The value of a square in a usable line is this.weight[line[this.player]+1]
        /// where line[this.player] is the number of pieces already placed.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="value"></param>
        /// <param name="opponent"></param>
        void Update(int[] line, int[] value, Player opponent)
        {
            var iPlayer = (int)this.player.Color;
            var iOpponent = (int)opponent.Color;

            /* If the opponent has no pieces in the line, then simply update the
             * value for player. 
             */
            if (line[iOpponent] == 0)
            {
                value[iPlayer] += this.weight[line[iPlayer] + 1] - this.weight[line[iPlayer]];
            }
            else
            {
                /* If it is the first piece in the line, then the line is
                 * spoiled for the opponent. 
                 */
                if (line[iPlayer] == 1)
                {
                    value[iOpponent] -= this.weight[line[iOpponent] + 1];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        Task<Tuple<int, int>> FindMove()
        {
            return Task.Run(() =>
            {
                var opponent = OpponentPlayer(this.player);
                var iOpponent = (int)opponent.Color;
                var iPlayer = (int)this.player.Color;

                int max = -10000;
                int value;

                // If no square has a high value then pick the one in the middle.
                var row = this.board.Size / 2;
                var column = this.board.Size / 2;

                if (this.board.IsEmpty(row, column))
                {
                    max = 4;
                }

                /* The evaluation for a square is simply the value of the square for
                 * the player (attack points) plus the value for the opponent
                 * (defense points). Attack is more important than defense, since it
                 * is better to get 5 in line yourself than to prevent the op- ponent
                 * from getting it. 
                 */

                var random = new Random((int)DateTime.Now.Ticks);

                // For all empty squares.
                for (int i = 0; i < this.board.Size; ++i)
                {
                    for (int j = 0; j < this.board.Size; ++j)
                    {
                        if (this.board.IsEmpty(i, j))
                        {
                            // compute evaluation
                            value = this.value[i][j][iPlayer] * (16 + this.attackFactor) / 16 +
                                this.value[i][j][iOpponent] + random.Next(4);

                            if (value > max)
                            {
                                row = i;
                                column = j;
                                max = value;
                            }
                        }
                    }
                }

                return new Tuple<int, int>(row, column);
            });
        }

        #region Debug methods

        /// <summary>
        /// 
        /// </summary>
        private void DumpBoard()
        {
            log.Debug("Board:");

            for (int i = 0; i < this.board.Size; ++i)
            {
                var sb = new System.Text.StringBuilder();
                for (int j = 0; j < this.board.Size; ++j)
                {
                    sb.AppendFormat("{0} ",
                        this.board.IsEmpty(i, j) ? '_' : this.board.Get(i, j));
                }
                log.Debug(sb.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        private void DumpLines(int player)
        {
            log.Debug("Lines for player {0}", player);
            for (int i = 0; i < 4; ++i)
            {
                log.Debug("Direction #{0}", i);
                for (int j = 0; j < this.board.Size; ++j)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int k = 0; k < this.board.Size; ++k)
                    {
                        sb.AppendFormat("{0} ", this.line[i][j][k][player]);
                    }
                    log.Debug(sb.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        private void DumpValues(int player)
        {
            log.Debug("Values for player {0}", player);
            for (int j = 0; j < this.board.Size; ++j)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int k = 0; k < this.board.Size; ++k)
                {
                    sb.AppendFormat("{0:000} ", this.value[j][k][player]);
                }
                log.Debug(sb.ToString());
            }
        }

        #endregion
    }
}