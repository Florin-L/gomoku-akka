using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gomoku.Common;

namespace GomokuClient.ViewModels
{
    /// <summary>
    /// The game view model.
    /// </summary>
    public class GameViewModel : GomokuClient.BindableBase
    {
        /// <summary>
        /// The white player.
        /// </summary>
        public Player WhitePlayer { get; set; }

        /// <summary>
        /// The black player.
        /// </summary>
        public Player BlackPlayer { get; set; }

        /// <summary>
        /// The player which is expected to make a move.
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// The board size.
        /// </summary>
        public int BoardSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GameViewModel()
        {
            WhitePlayer = new HumanPlayer()
            {
                Name = "White",
                Color = Color.White
            };

            BlackPlayer = new ComputerPlayer()
            {
                Name = "Black",
                Color = Color.Black
            };

            // The black player moves first.
            CurrentPlayer = BlackPlayer;

            BoardSize = 19;
        }
    }
}
