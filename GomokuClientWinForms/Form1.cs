using System;
using System.Windows.Forms;
using Akka.Actor;
using Gomoku.Common;

namespace GomokuClient
{
    public partial class Form1 : Form
    {
        Game game;

        public Form1()
        {
            InitializeComponent();

            this.game = new Game();

            this.game.WhitePlayer = new HumanPlayer("White", PlayerColor.White);
            this.game.BlackPlayer = new ComputerPlayer("Black", PlayerColor.Black);
            this.game.CurrentPlayer = this.game.BlackPlayer;
            this.game.BoardSize = this.boardCanvas.BoardSize;

            this.game.MoveMade += this.boardCanvas.OnMoveMade;
            this.game.PlayerChanged += this.OnPlayerChanged;
            this.game.GameStarted += this.OnGameStarted;
            this.game.GameOver += this.OnGameOver;

            this.game.GameReseted += this.boardCanvas.OnGameReseted;
            this.game.GameStarted += this.boardCanvas.OnGameStarted;
            this.game.GameOver += this.boardCanvas.OnGameOver;

            //
            this.connectionStatusLabel.Text = "Not connected";
            this.connectionStatusLabel.ForeColor = System.Drawing.Color.Black;

            this.game.ServerDisconnected += (sender, e) =>
            {
                this.connectionStatusLabel.Text = "Disconnected";
                this.connectionStatusLabel.ForeColor = System.Drawing.Color.Red;
            };

            this.game.ClientConnected += (sender, e) =>
            {
                this.connectionStatusLabel.Text = "Connected";
                this.connectionStatusLabel.ForeColor = System.Drawing.Color.Green;
            };
            //

            this.game.GameCreated += (sender, e) =>
            {
                this.connectionStatusLabel.Text = "Connected";
                this.connectionStatusLabel.ForeColor = System.Drawing.Color.Green;
            };

            //
            this.game.StartThinking += (sender, e) =>
            {
                this.playerThink.Text = string.Format("{0} think ...", e.PlayerName);
            };

            this.game.StopThinking += (sender, e) =>
            {
                this.playerThink.Text = "";
            };
            //

            this.boardCanvas.DataContext = this.game;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.turnPicture.Image = this.boardCanvas.BlackPieceImage;
        }

        #region Game event handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pce"></param>
        private void OnPlayerChanged(object sender, PlayerChangedEventArgs pce)
        {
            this.turnPicture.Image = 
                (pce.Player.Color == PlayerColor.White) ? this.boardCanvas.WhitePieceImage : this.boardCanvas.BlackPieceImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameStarted(object sender, GameStartedEventArgs e)
        {
            this.gameStatusLabel.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameOver(object sender, GameOverEventArgs e)
        {
            this.game.IsRunning = false;

            switch (e.Status)
            {
                case GameStatus.Draw:
                    MessageBox.Show("It's a draw");
                    this.gameStatusLabel.Text = "Draw !";
                    break;

                case GameStatus.WhiteWon:
                    MessageBox.Show("White won !");
                    this.gameStatusLabel.Text = "White won !";
                    break;

                case GameStatus.BlackWon:
                    MessageBox.Show("Black won !");
                    this.gameStatusLabel.Text = "Black won !";
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.GameActors.Terminate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new NewGameForm();
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                this.game.WhitePlayer = new Player()
                {
                    Name = form.WhitePlayer.Name,
                    Color = PlayerColor.White,
                    Type = form.WhitePlayer.Type,
                };

                this.game.BlackPlayer = new Player()
                {
                    Name = form.BlackPlayer.Name,
                    Color = PlayerColor.Black,
                    Type = form.BlackPlayer.Type,
                };

                //
                this.playerWhiteLabel.Text = this.game.WhitePlayer.Name;
                this.playerBlackLabel.Text = this.game.BlackPlayer.Name;

                // start the game
                var startGame = new Gomoku.Actors.Client.StartGame()
                {
                    Size = this.game.BoardSize,
                    WhitePlayer = this.game.WhitePlayer,
                    BlackPlayer = this.game.BlackPlayer
                };

                this.game.GameServiceActor.Tell(startGame);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.game.CancelGame();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented !");
        }
    }
}
