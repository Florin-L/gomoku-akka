using Gomoku.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NLog;

namespace GomokuClient
{
    public partial class BoardCanvas : Panel
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public int BoardSize
        {
            get { return this.boardSize; }
            set
            {
                this.boardSize = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CellSize
        {
            get { return this.cellSize; }
            private set { this.cellSize = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Image WhitePieceImage
        {
            get
            {
                if (this.whitePieceImage == null)
                {
                    this.whitePieceImage = new Bitmap(cellSize, cellSize);
                    using (var g = Graphics.FromImage(this.whitePieceImage))
                    {
                        g.DrawImage(this.atlasImages, 
                            new Rectangle(0, 0, cellSize, cellSize), this.whitePieceRect, GraphicsUnit.Pixel);
                    }
                }
                return this.whitePieceImage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Image BlackPieceImage
        {
            get
            {
                if (this.blackPieceImage == null)
                {
                    this.blackPieceImage = new Bitmap(cellSize, cellSize);
                    using (var g = Graphics.FromImage(this.blackPieceImage))
                    {
                        g.DrawImage(this.atlasImages, new Rectangle(0, 0, cellSize, cellSize),
                            this.blackPieceRect, GraphicsUnit.Pixel);
                    }
                }
                return this.blackPieceImage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object DataContext { get; set; }

        #region Images

        Image atlasImages;
        Image boardImage;
        Image whitePieceImage;
        Image blackPieceImage;

        Rectangle movingRect;
        Rectangle lastPieceRect;
        Rectangle pieceRect;
        Rectangle whitePieceRect;
        Rectangle blackPieceRect;

        #endregion

        List<GameMove> winningLine;
        Timer blinkTimer;
        bool changed;

        Logger log = LogManager.GetCurrentClassLogger();

        int cellSize = 24;
        int boardSize = 19;

        /// <summary>
        /// 
        /// </summary>
        public BoardCanvas()
        {
            this.DoubleBuffered = true;
            this.InitializeComponent();

            //
            this.changed = false;
            this.blinkTimer = new Timer();
            this.blinkTimer.Interval = 300;

            this.blinkTimer.Tick += OnBlinkTime;

            //
            this.atlasImages = Properties.Resources.HGarden;
            this.CreateBoardImage();

            //
            this.lastPieceRect = 
                new Rectangle(
                    this.boardSize/2 * this.cellSize, this.boardSize/2 * this.cellSize, this.cellSize, this.cellSize);

            this.movingRect = 
                new Rectangle(this.boardSize / 2, this.boardSize / 2, this.cellSize, this.cellSize);

            this.pieceRect = new Rectangle(0, 0, this.cellSize, this.cellSize);

            this.blackPieceRect = new Rectangle(this.cellSize, 0, this.cellSize, this.cellSize);

            this.whitePieceRect = new Rectangle(2 * this.cellSize, 0, this.cellSize, this.cellSize);
        }

        #region Event Handlers

        /// <summary>
        /// Handles Paint event.
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var g = pe.Graphics;

            // draws the board
            this.DrawBoard(g);

            var game = (Game)this.DataContext;
            if (game == null)
            {
                return;
            }

            // draws the pieces
            foreach (var gm in game.Moves)
            {
                this.DrawPiece(g, gm.Row, gm.Column, gm.Color);
            }

            // marks the position of the last piece placed on the table            
            g.DrawRectangle(Pens.Crimson, this.lastPieceRect);

            //
            if (game.IsRunning && game.IsHumanPlayerTurn())
            {
                g.DrawRectangle(Pens.BlueViolet, this.movingRect);
            }
        }

        /// <summary>
        /// Handles MouseClick event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var row = e.Y / this.cellSize;
            var column = e.X / this.cellSize;

            var gameContext = (Game)this.DataContext;
            gameContext.ValidateMove(new GameMove(row, column));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.movingRect.Y = (e.Y / this.cellSize) * this.cellSize;
            this.movingRect.X = (e.X / this.cellSize) * this.cellSize;
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mve"></param>
        internal void OnMoveMade(object sender, MoveEventArgs mve)
        {
            this.lastPieceRect.X = mve.GameMove.Column * this.cellSize;
            this.lastPieceRect.Y = mve.GameMove.Row * this.cellSize;
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnGameReseted(object sender, EventArgs e)
        {
            this.blinkTimer.Stop();

            if (this.winningLine != null)
            {
                this.winningLine.Clear();
                this.winningLine = null;
            }

            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnGameStarted(object sender, GameStartedEventArgs e)
        {
            Refresh();

            var game = this.DataContext as Game;
            if (game.BlackPlayer.IsHuman())
            {
                game.ValidateMove(new GameMove(this.boardSize / 2, this.boardSize / 2));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnGameOver(object sender, GameOverEventArgs e)
        {
            if (e.WinningLineCoords != null)
            {
                var game = (Game)this.DataContext;

                this.winningLine = new List<GameMove>();
                foreach (var t in e.WinningLineCoords)
                {
                    this.winningLine.Add(new GameMove()
                    {
                        Row = t.Item1,
                        Column = t.Item2,
                        Color = game.Winner.Color
                    });
                }

                // blink the pieces
                this.blinkTimer.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBlinkTime(object sender, EventArgs e)
        {
            var game = (Game)this.DataContext;
            foreach (var gm in this.winningLine)
            {
                if (!changed)
                {
                    game.Moves.Remove(gm);
                }
                else
                {
                    game.Moves.Add(gm);
                }
            }
            changed = !changed;
            this.Refresh();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="color"></param>
        private void DrawPiece(Graphics g, int row, int column, PlayerColor color)
        {
            var srcRect = (color == PlayerColor.White) ? this.whitePieceRect : this.blackPieceRect;

            this.pieceRect.X = column * this.cellSize;
            this.pieceRect.Y = row * this.cellSize;

            g.DrawImage(this.atlasImages, this.pieceRect, srcRect, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Draws the board.
        /// </summary>
        /// <param name="g"></param>
        private void DrawBoard(Graphics g)
        {
            g.DrawImage(this.boardImage, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateBoardImage()
        {
            this.boardImage = new Bitmap(this.cellSize * boardSize, this.cellSize * this.boardSize);

            using (var g = Graphics.FromImage(this.boardImage))
            {
                var srcRect = new Rectangle(0, 0, this.cellSize, this.cellSize);
                var dstRect = new Rectangle(0, 0, this.cellSize, this.cellSize);

                for (int x = 0; x < this.BoardSize; x++)
                {
                    for (int y = 0; y < this.BoardSize; y++)
                    {
                        dstRect.X = x * this.cellSize;
                        dstRect.Y = y * this.cellSize;

                        g.DrawImage(this.atlasImages, dstRect, srcRect, GraphicsUnit.Pixel);
                    }
                }
            }
        }
    }
}
