using Gomoku.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku.Actors.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class StartGame
    {
        public int Size { get; set; }
        public Player WhitePlayer { get; set; }
        public Player BlackPlayer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StartGame() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="whitePlayer"></param>
        /// <param name="blackPlayer"></param>
        public StartGame(int size, Player whitePlayer, Player blackPlayer)
        {
            Size = size;
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Move
    {
        public Player Player { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Move() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Move(Player player, int row, int column)
        {
            Player = player;
            Row = row;
            Column = column;
        }
    }
}