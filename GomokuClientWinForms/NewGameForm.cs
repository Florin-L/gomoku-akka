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

namespace GomokuClient
{
    public partial class NewGameForm : Form
    {
        public Player WhitePlayer { get; private set; }
        public Player BlackPlayer { get; private set; }

        public NewGameForm()
        {
            InitializeComponent();

            this.WhitePlayer = new HumanPlayer("Human", PlayerColor.White);
            this.BlackPlayer = new ComputerPlayer("Computer", PlayerColor.Black, 4);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.WhitePlayer = new Player()
            {
                Name = this.whitePlayerName.Text,
                Color = PlayerColor.White,
                Type = this.whitePlayerName.Text.Equals("Human") ? PlayerType.Human : PlayerType.Computer
            };

            this.BlackPlayer = new Player()
            {
                Name = this.blackPlayerName.Text,
                Color = PlayerColor.Black,
                Type = this.blackPlayerName.Text.Equals("Human") ? PlayerType.Human : PlayerType.Computer
            };
        }

        private void btnSwitchSides_Click(object sender, EventArgs e)
        {
            //
            this.whitePlayerName.SelectedIndex = 1;
            this.blackPlayerName.SelectedIndex = 0;

            //
            var value = this.whiteSearchDepth.Value;
            this.whiteSearchDepth.Value = this.blackSearchDepth.Value;
            this.blackSearchDepth.Value = value;

            this.whiteSearchDepth.Enabled = !this.whitePlayerName.SelectedText.Equals("Human");
            this.blackSearchDepth.Enabled = !this.whiteSearchDepth.Enabled;

            //
            var value2 = whiteTimeLimit.Value;
            this.whiteTimeLimit.Value = blackTimeLimit.Value;
            this.blackTimeLimit.Value = value2;
        }

        private void NewGameForm_Load(object sender, EventArgs e)
        {
            this.whitePlayerName.SelectedIndex = 0;
            this.blackPlayerName.SelectedIndex = 1;

            this.whiteSearchDepth.Value = 0;
            this.blackSearchDepth.Value = 4;

            this.whiteTimeLimit.Value = 0;
            this.blackTimeLimit.Value = 0;
        }
    }
}
