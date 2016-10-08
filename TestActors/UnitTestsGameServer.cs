using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Akka.Actor;
using Akka.TestKit.VsTest;

using Gomoku.Common;
using Gomoku.Actors;

namespace TestActors
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class UnitTest_GameServer : TestKit
    {
        [TestCleanup]
        public void Cleanup()
        {
            Shutdown();
        }

        [TestInitialize]
        public void Setup()
        {

        }

        [TestMethod]
        public void TestCreateGameServerActor()
        {
            // creates the game server actor
            var serverActor = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActor);
        }

        [TestMethod]
        public void TestCreateGame_WhiteIsHuman()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var createGame = new CreateGame(size: 15,
                white: new HumanPlayer("white", PlayerColor.White),
                black: new ComputerPlayer("black", PlayerColor.Black, 2));

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>().Guid;
            Assert.IsNotNull(answer);
        }

        [TestMethod]
        public void TestStartGame_WhiteIsComputer()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var createGame = new CreateGame(size: 15,
                white: new ComputerPlayer("white", PlayerColor.White, 4),
                black: new HumanPlayer("black", PlayerColor.Black));

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsNotNull(answer.GameActorRef);

            answer.GameActorRef.Tell(new StartGame(answer.Guid));
            ExpectMsg<GameStarted>();
        }

        [TestMethod]
        public void TestCreateGame_BlackIsHuman()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var createGame = new CreateGame(size: 15,
                white: new ComputerPlayer("white", PlayerColor.White, 2),
                black: new HumanPlayer("black", PlayerColor.Black));

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>().Guid;
            Assert.IsNotNull(answer);
        }

        [TestMethod]
        public void TestStartGame_BlackIsComputer()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var createGame = new CreateGame(size: 15,
                white: new HumanPlayer("white", PlayerColor.White),
                black: new ComputerPlayer("black", PlayerColor.Black, 2));

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsNotNull(answer.GameActorRef);

            answer.GameActorRef.Tell(new StartGame(answer.Guid));
            ExpectMsg<GameStarted>();

            var answer2 = ExpectMsg<MoveMade>();
            Assert.IsNotNull(answer2);
            Assert.IsTrue((answer2.Row == 7) && (answer2.Column == 7));
        }

        [TestMethod]
        public void Test_WhiteMove()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var player = new HumanPlayer("white", PlayerColor.White);

            var createGame = new CreateGame(size: 19,
                white: player,
                black: new ComputerPlayer("black", PlayerColor.Black, 2));

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsNotNull(answer.GameActorRef);

            var gameActor = answer.GameActorRef;
            answer.GameActorRef.Tell(new StartGame(answer.Guid));
            ExpectMsg<GameStarted>();

            var answer2 = ExpectMsg<MoveMade>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsTrue((answer2.Row == 9) && (answer2.Column == 9));

            gameActor.Tell(new MoveMade(answer.Guid, player, 0, 0));
            var answer3 = ExpectMsg<MoveResponse>();
            Assert.IsNotNull(answer2.Guid);
            Assert.IsTrue(answer3.MoveStatus == MoveStatus.Accepted);
            Assert.IsTrue(answer3.NextPlayer.Color == PlayerColor.Black);
        }

        [TestMethod]
        public void Test_WhiteMove_Rejected()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var player = new HumanPlayer("white", PlayerColor.White);

            var createGame = new CreateGame(size: 19,
                white: player,
                black: new ComputerPlayer("black", PlayerColor.Black, 2));

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsNotNull(answer.GameActorRef);

            var gameActor = answer.GameActorRef;
            answer.GameActorRef.Tell(new StartGame(answer.Guid));
            ExpectMsg<GameStarted>();

            var answer2 = ExpectMsg<MoveMade>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsTrue((answer2.Row == 9) && (answer2.Column == 9));

            gameActor.Tell(new MoveMade(answer2.Guid, player, 9, 9));
            var answer3 = ExpectMsg<MoveResponse>();
            Assert.IsNotNull(answer3.Guid);
            Assert.IsTrue(answer3.MoveStatus == MoveStatus.Rejected);
        }

        [TestMethod]
        public void Test_BlackMove()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var player = new HumanPlayer("black", PlayerColor.Black);

            var createGame = new CreateGame()
            {
                Size = 19,
                White = new ComputerPlayer("white", PlayerColor.White, 2),
                Black = player
            };

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsNotNull(answer.GameActorRef);

            var gameActor = answer.GameActorRef;
            answer.GameActorRef.Tell(new StartGame(answer.Guid));
            ExpectMsg<GameStarted>();

            // black's turn
            gameActor.Tell(new MoveMade(answer.Guid, player, 9, 9));
            var answer2 = ExpectMsg<MoveResponse>();
            Assert.IsNotNull(answer2.Guid);
            Assert.IsTrue(answer2.MoveStatus == MoveStatus.Accepted);
            Assert.IsTrue(answer2.GameStatus == GameStatus.Continue);
            Assert.IsTrue(answer2.NextPlayer.Color == PlayerColor.White);
        }

        [TestMethod]
        public void Test_BlackMove_Rejected()
        {
            var serverActorRef = ActorOfAsTestActorRef<GameServerActor>();
            Assert.IsNotNull(serverActorRef);

            var player = new HumanPlayer("black", PlayerColor.Black);

            var createGame = new CreateGame(size: 19,
                white: new ComputerPlayer("white", PlayerColor.White, 2),
                black: player);

            serverActorRef.Tell(createGame, TestActor);
            var answer = ExpectMsg<CreateGameResponse>();
            Assert.IsNotNull(answer.Guid);
            Assert.IsNotNull(answer.GameActorRef);

            var gameActor = answer.GameActorRef;
            answer.GameActorRef.Tell(new StartGame(answer.Guid));
            ExpectMsg<GameStarted>();

            // black's turn
            gameActor.Tell(new MoveMade(answer.Guid, player, 0, 0));

            var answer3 = ExpectMsg<MoveResponse>();
            Assert.IsNotNull(answer3.Guid);
            Assert.IsTrue(answer3.MoveStatus == MoveStatus.Accepted);
            Assert.IsTrue(answer3.NextPlayer.Color == PlayerColor.White);

            // white's response
            var answer4 = ExpectMsg<MoveMade>();
            Assert.IsNotNull(answer4.Guid);

            var row = answer4.Row;
            var column = answer4.Column;

            // black's turn
            gameActor.Tell(new MoveMade(answer4.Guid, player, row, column));

            var answer5 = ExpectMsg<MoveResponse>();
            Assert.IsNotNull(answer5.Guid);
            Assert.IsTrue(answer5.MoveStatus == MoveStatus.Rejected);
            Assert.IsTrue(answer5.NextPlayer.Color == PlayerColor.Black);
        }
    }
}
