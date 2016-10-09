using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using Akka.Actor;
using Gomoku.Actors;
using Gomoku.Actors.Client;

namespace GomokuClient.Actors
{
    /// <summary>
    /// 
    /// </summary>
    class GameServerClientActor : ReceiveActor
    {
        #region States

        ActorSelection gameServiceSelection;
        IActorRef gameServiceRef;
        Game game;
        Logger log = LogManager.GetCurrentClassLogger();
        bool isCancelling = false;
        //bool isGameOver = false;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public GameServerClientActor(Game game)
        {
            this.game = game;

            Receive<CreateGameResponse>(message => this.HandleCreateGameResponse(message));
            Receive<Gomoku.Actors.Client.StartGame>(message => this.HandleStartGame(message));
            Receive<GameStarted>(message => this.HandleGameStarted(message));
            Receive<CancelGame>(message => this.HandleCancelGame(message));
            Receive<GameCancelled>(message => this.HandleGameCancelled(message));

            Receive<Move>(message => this.HandleClientMove(message));
            Receive<MoveResponse>(message => this.HandleMoveResponse(message));

            Receive<ClientConnected>(message => this.HandleClientConnected(message));

            Receive<Terminated>(message => this.HandleGameServerActorTerminated(message));
            Receive<PingResponse>(message => this.HandlePingResponse(message));

            Receive<StartThinking>(message => this.HandleStartThinking(message));
            Receive<StopThinking>(message => this.HandleStopThinking(message));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void PreStart()
        {
            // connects to the remote actor
            this.gameServiceSelection = Program.GameActors.ActorSelection(
                @"akka.tcp://GomokuActorSystem@localhost:9000/user/GameService");

            this.gameServiceSelection.Tell(new ClientConnect());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStartGame(Gomoku.Actors.Client.StartGame message)
        {
            this.log.Debug("GameServerClientActor - received StartGame");

            var createGame = new CreateGame()
            {
                Size = message.Size,
                White = message.WhitePlayer,
                Black = message.BlackPlayer
            };

            this.log.Debug("White player: {0}, Black player {1}", 
                message.WhitePlayer, message.BlackPlayer);

            this.gameServiceSelection.Tell(createGame);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleCreateGameResponse(CreateGameResponse message)
        {
            if (message.Status)
            {
                this.log.Info("GameServerClientActor - a new game has been created; guid={0}", message.Guid);

                // DeadWatch the game service.
                this.gameServiceRef = Sender;
                Context.Watch(this.gameServiceRef);

                //
                this.game.Guid = message.Guid;
                //
                this.game.OnGameCreated(message.Guid);

                // start the game
                this.gameServiceSelection.Tell(new Gomoku.Actors.StartGame(message.Guid));
            }
            else
            {
                this.log.Error("GameServerClientActor - failed to create a new game. Error: {0}", message.ErrorMessage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private void HandleGameStarted(GameStarted message)
        {
            this.game.OnGameStarted(message.Guid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleCancelGame(CancelGame message)
        {
            this.log.Info("Cancelling the game {0}", message.Guid);
            // send the cancellation message to the game server
            this.gameServiceSelection.Tell(message);

            this.isCancelling = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleGameCancelled(GameCancelled message)
        {
            this.log.Info("The game {0} has been cancelled.", message.Guid);

            // unwatch
            Context.Unwatch(this.gameServiceRef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleClientMove(Move message)
        {
            if (this.isCancelling)
            {
                return;
            }

            this.gameServiceSelection.Tell(
                new MakeMove(this.game.Guid, message.Player, message.Row, message.Column));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleMoveResponse(MoveResponse message)
        {
            if (this.isCancelling)
            {
                return;
            }

            this.game.OnMoveResponse(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleClientConnected(ClientConnected message)
        {
            this.game.OnClientConnected(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleGameServerActorTerminated(Terminated message)
        {
            if (message.ActorRef == this.gameServiceRef)
            {
                if (message.AddressTerminated || message.ExistenceConfirmed)
                {
                    this.game.OnServerDisconnected();
                }

                if (message.AddressTerminated)
                {
                    this.log.Fatal("The game service has been disassociated.");
                }

                if (message.ExistenceConfirmed)
                {
                    this.log.Info("The game service terminated gracefully.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandlePingResponse(PingResponse message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStopThinking(StopThinking message)
        {
            this.game.OnStopThinking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandleStartThinking(StartThinking message)
        {
            this.game.OnStartThinking();
        }
    }
}
