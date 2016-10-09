using System;
using System.Collections.Generic;

using NLog;

using Akka.Actor;
using Gomoku.Common;
using Gomoku.Actors;

namespace GomokuClient
{
    /// <summary>
    /// MoveEvent handler.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="me"></param>
    internal delegate void MoveEventHandler(object sender, MoveEventArgs me);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="pce"></param>
    internal delegate void PlayerChangedEventHandler(object sender, PlayerChangedEventArgs pce);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void GameCreatedEventHandler(object sender, GameCreatedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void GameStartedEventHandler(object sender, GameStartedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void ResetGameEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void GameOverEventHandler(object sender, GameOverEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServerDisconnectedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ClientConnectedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sener"></param>
    /// <param name="e"></param>
    internal delegate void StartThinkingEventHandler(object sener, StartThinkingEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sener"></param>
    /// <param name="e"></param>
    internal delegate void StopThinkingEventHandler(object sener, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    internal class MoveEventArgs : EventArgs
    {
        public GameMove GameMove { get; set; }

        public MoveEventArgs() { }

        public MoveEventArgs(GameMove gm)
        {
            this.GameMove = gm;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PlayerChangedEventArgs : EventArgs
    {
        public Player Player { get; set; }

        public PlayerChangedEventArgs() { }

        public PlayerChangedEventArgs(Player player)
        {
            this.Player = player;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class GameCreatedEventArgs : EventArgs
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// The default contstructor.
        /// </summary>
        public GameCreatedEventArgs() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="guid"></param>
        public GameCreatedEventArgs(Guid guid)
        {
            this.Guid = guid;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class GameStartedEventArgs : EventArgs
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// The default contstructor.
        /// </summary>
        public GameStartedEventArgs() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="guid"></param>
        public GameStartedEventArgs(Guid guid)
        {
            this.Guid = guid;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class GameOverEventArgs : EventArgs
    {
        public GameStatus Status { get; set; }
        public Player Winner { get; set; }
        public WinningLine WinningLine { get; set; }
        public IList<Tuple<int, int>> WinningLineCoords { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public GameOverEventArgs()
        {
            WinningLine = WinningLine.None;
            WinningLineCoords = null;
            Winner = null;
        }

        public GameOverEventArgs(GameStatus status, WinningLine winningLine, 
            int row, int column) : this()
        {
            this.Status = status;
            this.WinningLine = winningLine;
            this.Row = row;
            this.Column = column;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class GameMove
    {
        public int Index { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public PlayerColor Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GameMove() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        public GameMove(int index, int row, int column, PlayerColor color) : this(row, column)
        {
            this.Index = index;
            this.Color = color;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public GameMove(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var that = (GameMove)obj;
            return (this.Row == that.Row) && 
                (this.Column == that.Column) && 
                (this.Color == that.Color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class StartThinkingEventArgs : EventArgs
    {
        internal string PlayerName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Game
    {
        #region Properties

        public int BoardSize { get; set; }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }
        public Player CurrentPlayer { get; set; }
        public Player Winner { get; set; }
        internal List<GameMove> Moves { get; set; }
        public bool IsRunning { get; set; }
        public GameStatus Status { get; set; }

        /// <summary>
        /// The reference to the game actor client.
        /// </summary>
        public IActorRef GameServiceActor { get; set; }

        /// <summary>
        /// The Guid of the current game.
        /// </summary>
        public Guid Guid { get; set; }

        #endregion

        #region States

        int index;
        Logger log = LogManager.GetCurrentClassLogger();

        #endregion

        #region Events

        internal event MoveEventHandler MoveMade;
        internal event PlayerChangedEventHandler PlayerChanged;
        internal event GameCreatedEventHandler GameCreated;
        internal event GameStartedEventHandler GameStarted;
        internal event ResetGameEventHandler GameReseted;
        internal event GameOverEventHandler GameOver;
        internal event ServerDisconnectedEventHandler ServerDisconnected;
        internal event ClientConnectedEventHandler ClientConnected;
        internal event StartThinkingEventHandler StartThinking;
        internal event StopThinkingEventHandler StopThinking;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Game()
        {
            this.Moves = new List<GameMove>();
            this.Status = GameStatus.Continue;

            this.GameServiceActor = Program.GameActors.ActorOf(
                Props.Create(() => new GomokuClient.Actors.GameServerClientActor(this)).
                WithDispatcher("akka.actor.synchronized-dispatcher"));
        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnServerDisconnected()
        {
            this.ServerDisconnected?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        internal void OnGameStarted(Guid guid)
        {
            this.index = 0;
            this.Moves.Clear();
            this.IsRunning = true;
            this.Winner = null;
            this.CurrentPlayer = this.BlackPlayer;
            this.GameStarted?.Invoke(this, new GameStartedEventArgs(guid));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        internal void OnMoveResponse(MoveResponse response)
        {
            if (response.MoveStatus == MoveStatus.Accepted)
            {
                var mv = new GameMove()
                {
                    Index = ++this.index,
                    Row = response.Row,
                    Column = response.Column,
                    Color = CurrentPlayer.Color
                };

                // stores the move
                this.Move(mv);

                //
                this.Status = response.GameStatus;
                this.Winner = response.Player;

                if (response.GameStatus != GameStatus.Continue)
                {
                    this.GameOver?.Invoke(this,
                        new GameOverEventArgs()
                        {
                            Status = this.Status,
                            Winner = this.Winner,
                            WinningLine = response.WinningLine,
                            WinningLineCoords = response.WinningLineCoords,
                            Row = response.Row,
                            Column = response.Column
                        });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        internal void OnClientConnected(ClientConnected message)
        {
            this.ClientConnected?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        internal void CancelGame()
        {
            this.GameServiceActor.Tell(new CancelGame(Guid));
            this.Moves.Clear();
            this.IsRunning = false;
            this.GameReseted?.Invoke(this, new EventArgs());

            // notify the listeners
            this.PlayerChanged?.Invoke(this,
                new PlayerChangedEventArgs(
                    new Player(
                        CurrentPlayer.Name, CurrentPlayer.Type, CurrentPlayer.Color, 
                        CurrentPlayer.SearchDepth, CurrentPlayer.TimeLimit)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gm"></param>
        internal void OnMove(GameMove move)
        {
            var mv = new GameMove()
            {
                Index = ++this.index,
                Row = move.Row,
                Column = move.Column,
                Color = CurrentPlayer.Color
            };

            this.Move(mv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gm"></param>
        internal void PlayerMove(GameMove gm)
        {
            this.GameServiceActor.Tell(
                new Gomoku.Actors.Client.Move(this.CurrentPlayer, gm.Row, gm.Column));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gm"></param>
        internal void Move(GameMove move)
        {
            this.log.Debug("Move accepted: index={0}, color={1}, row={2}, column={3}",
                move.Index, move.Color, move.Row, move.Column);

            // add the move to our moves list
            this.Moves.Add(move);

            // notify the subscribers
            this.MoveMade?.Invoke(this, new MoveEventArgs(move));

            // change the current player
            this.CurrentPlayer = 
                (this.CurrentPlayer == this.WhitePlayer) ? this.BlackPlayer : this.WhitePlayer;

            // notify the listeners
            this.PlayerChanged?.Invoke(this,
                new PlayerChangedEventArgs(
                    new Player(
                        this.CurrentPlayer.Name, this.CurrentPlayer.Type, this.CurrentPlayer.Color,
                        this.CurrentPlayer.SearchDepth, this.CurrentPlayer.TimeLimit)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        internal void OnGameCreated(Guid guid)
        {
            this.GameReseted?.Invoke(this, new EventArgs());
            this.GameCreated?.Invoke(this, new GameCreatedEventArgs() { Guid = guid });
        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnStartThinking()
        {
            this.StartThinking?.Invoke(this, 
                new StartThinkingEventArgs() { PlayerName = GetComputerPlayer().Name });
        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnStopThinking()
        {
            this.StopThinking?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool IsHumanPlayerTurn()
        {
            var wp = GetHumanPlayer();
            if (wp == null)
            {
                return false;
            }

            return wp == this.CurrentPlayer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Player GetHumanPlayer()
        {
            if (this.WhitePlayer.IsHuman() || this.BlackPlayer.IsHuman())
            {
                return this.WhitePlayer.IsHuman() ? this.WhitePlayer : this.BlackPlayer;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Player GetComputerPlayer()
        {
            if (this.WhitePlayer.IsComputer() || this.BlackPlayer.IsComputer())
            {
                return this.WhitePlayer.IsComputer() ? this.WhitePlayer : this.BlackPlayer;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Player GetNextPlayer()
        {
            return this.CurrentPlayer == this.WhitePlayer ? this.BlackPlayer : this.WhitePlayer;
        }
    }
}
