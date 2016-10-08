using System;
using System.Collections.Generic;

using Akka.Actor;
using Gomoku.Common;

namespace Gomoku.Actors
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <param name="message"></param>
        public static void NotifyPlayers(this IActorRef self,
            IActorRef player1, IActorRef player2, GameMessageBase message)
        {
            if (player1 != null)
            {
                player1.Tell(message, self);
            }

            if (player2 != null)
            {
                player2.Tell(message, self);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="listeners"></param>
        /// <param name="message"></param>
        public static void NotifyListeners(this IActorRef self, 
            IList<IActorRef> listeners, GameMessageBase message)
        {
            foreach (var listener in listeners)
            {
                if (listener != null)
                {
                    listener.Tell(message, self);
                }
            }
        }
    }
}
