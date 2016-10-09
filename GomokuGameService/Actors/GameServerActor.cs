using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using Akka.Actor;
using Gomoku.Common;

namespace Gomoku.Actors
{
    /// <summary>
    /// 
    /// </summary>
    internal class GameContext
    {
        public IActorRef ClientActorRef { get; set; }
        public IActorRef GameActorRef { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GameServerActor : ReceiveActor
    {
        #region States

        readonly IDictionary<Guid, GameContext> games = new Dictionary<Guid, GameContext>();
        readonly Logger log = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// The constructor.
        /// </summary>
        public GameServerActor()
        {
            Receive<CreateGame>(message => this.HandleGameCreate(message));
            Receive<StartGame>(message => this.HandleStartGame(message));
            Receive<GameStarted>(message => this.HandleGameStarted(message));
            Receive<CancelGame>(message => this.HandleCancelGame(message));

            Receive<ClientConnect>(message => Sender.Tell(new ClientConnected()));
            Receive<Ping>(message => Sender.Tell(new PingResponse()));

            Receive<MakeMove>(message => this.HandleMakeMove(message));
            Receive<MoveResponse>(message => this.HandleMoveResponse(message));

            Receive<StartThinking>(message => this.HandleStartThinking(message));
            Receive<StopThinking>(message => this.HandleStopThinking(message));
        }

        #region Message Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(maxNrOfRetries: 5, withinTimeMilliseconds: 2000,
                decider: Decider.From(x =>
                {
                    return Directive.Stop;
                }));
        }

        /// <summary>
        /// Handles GameCreate message.
        /// </summary>
        /// <param name="message"></param>
        private void HandleGameCreate(CreateGame message)
        {
            this.log.Info("Received CreateGame message.");

            var guid = Guid.NewGuid();
            try
            {
                // create a new game actor
                Props actorProps = Props.Create(
                    () => new GameActor(
                        Self,               // the game server actor
                        guid,
                        message.Size,
                        message.White,
                        message.Black));

                var gameActor = Context.ActorOf(actorProps);

                // add the new actor to the list of the game actors 
                this.games.Add(guid, 
                    new GameContext() { ClientActorRef = Sender, GameActorRef = gameActor });

                this.log.Info("The game {0} has been created.", guid);
                this.log.Debug("White: {0}, Black: {1}", message.White, message.Black);

                this.Sender.Tell(new CreateGameResponse(guid, gameActor));
            }
            catch (Exception e)
            {
                this.Sender.Tell(new Failure() { Exception = e });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStartGame(StartGame message)
        {
            this.log.Info("StartGame - game {0}", message.Guid);

            GameContext actors;
            this.games.TryGetValue(message.Guid, out actors);
            if (actors != null)
            {
                actors.GameActorRef.Tell(message, Sender);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleGameStarted(GameStarted message)
        {
            this.log.Info("Game {0} has started.", message.Guid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleCancelGame(CancelGame message)
        {
            this.log.Info("Cancelling game {0}", message.Guid);

            GameContext actors;
            this.games.TryGetValue(message.Guid, out actors);

            if (actors != null)
            {
                this.games.Remove(message.Guid);
                this.log.Info("Game {0} has been removed from map.", message.Guid);

                actors.GameActorRef.Tell(new GameCancelled(message.Guid));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleMakeMove(MakeMove message)
        {
            this.log.Debug("MakeMove - game={0}, player={1}, row={2}, column={3}",
                message.Guid, message.Player, message.Row, message.Column);

            GameContext actors;
            this.games.TryGetValue(message.Guid, out actors);
            if (actors != null)
            {
                actors.GameActorRef.Tell(message, Sender);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleMoveMade(MoveMade message)
        {
            this.log.Debug("MoveMade - game={0}, player={1}, row={2}, column={3}", 
                message.Guid, message.Player, message.Row, message.Column);

            GameContext actors;
            this.games.TryGetValue(message.Guid, out actors);
            if (actors != null)
            {
                actors.GameActorRef.Tell(message, Sender);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleMoveResponse(MoveResponse message)
        {
            this.log.Debug("MoveResponse - game={0}, player={1}, status={2}", 
                message.Guid, message.Player, message.GameStatus);

            SendMessageToGameClient(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStopThinking(StopThinking message)
        {
            SendMessageToGameClient(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStartThinking(StartThinking message)
        {
            SendMessageToGameClient(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        private void SendMessageToGameClient(GameMessageBase message)
        {
            GameContext actors;
            this.games.TryGetValue(message.Guid, out actors);
            if (actors != null)
            {
                actors.ClientActorRef.Tell(message);
            }
        }

        #endregion
    }
}
