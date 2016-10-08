using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Gomoku.Common;

namespace Gomoku.Actors
{
    /// <summary>
    /// 
    /// </summary>
    internal class SimpleBrainActor : ReceiveActor
    {
        /// <summary>
        /// 
        /// </summary>
        internal class StartThink
        {
        }

        #region State

        GameActor.Board board;
        PlayerColor playerColor;

        /* The number of Empty lines left */
        int totalLines = 0;

        /* Importance of attack (1..16) */
        int attackFactor;

        /* Value of having 0, 1,2,3,4 or 5 pieces in line */
        int[] weight = { 0, 0, 4, 20, 100, 500, 0 };

        /* Value of each square for each player */
        int[,,] value;

        int[,,,] line;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public SimpleBrainActor()
        {
            this.board = null;
            this.totalLines = 0;
            this.attackFactor = 4;

            Receive<StartThink>(m => HandleStartThink(m));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="board"></param>
        public SimpleBrainActor(GameActor.Board board, PlayerColor playerColor, int attackFactor)
            : this()
        {
            this.board = board;
            this.playerColor = playerColor;
            this.attackFactor = attackFactor;

            this.value = new int[board.Size + 1, board.Size + 1, (int)(PlayerColor.Black) + 1];
            this.line = new int[4, board.Size + 1, board.Size + 1, (int)(PlayerColor.Black) + 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private void HandleStartThink(StartThink m)
        {
            throw new NotImplementedException();
        }
    }
}
