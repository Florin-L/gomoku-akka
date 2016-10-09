
using System;
using System.Collections.Generic;
using Gomoku.Common;
using Akka.Actor;

/// <summary>
/// The classes in this file implement the game messages 
/// which are sent between the client actors and the game server actor.
/// </summary>

namespace Gomoku.Actors
{
    /// <summary>
    /// Creates a new game.
    /// </summary>
    public class CreateGame
    {
        public int Size { get; set; }
        public Player White { get; set; }
        public Player Black { get; set; }

        public CreateGame() { }

        public CreateGame(int size, Player white, Player black)
        {
            Size = size;
            White = white;
            Black = black;
        }
    }

    /// <summary>
    /// The base class for the game messages.
    /// </summary>
    public class GameMessageBase
    {
        public Guid Guid { get; set; }

        public GameMessageBase() { }

        public GameMessageBase(Guid guid)
        {
            this.Guid = guid;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClientConnect
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class ClientConnected { }

    /// <summary>
    /// 
    /// </summary>
    public class Ping { }

    /// <summary>
    /// 
    /// </summary>
    public class PingResponse { }

    /// <summary>
    /// The response to the 'GameCreate' request.
    /// </summary>
    public class CreateGameResponse : GameMessageBase
    {
        public IActorRef GameActorRef { get; set; }

        /// <summary>
        /// The status: true if the game was created; false otherwise.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// The error message set when the creating of a game failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public CreateGameResponse() { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="gameActorRef"></param>
        /// <param name="status"></param>
        /// <param name="errorMessage"></param>
        public CreateGameResponse(Guid guid, IActorRef gameActorRef = null,
            bool status = true, string errorMessage = "")
            : base(guid)
        {
            GameActorRef = gameActorRef;
            Status = status;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Starts a new created game.
    /// </summary>
    public class StartGame : GameMessageBase
    {
        public StartGame() { }
        public StartGame(Guid guid) : base(guid) { }
    }

    /// <summary>
    /// The response to the 'StartGame' request.
    /// </summary>
    public class GameStarted : GameMessageBase
    {
        public GameStarted() { }
        public GameStarted(Guid guid) : base(guid) { }
    }

    /// <summary>
    /// Cancels a running game.
    /// </summary>
    public class CancelGame : GameMessageBase
    {
        public CancelGame() { }
        public CancelGame(Guid guid) : base(guid) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GameCancelled : GameMessageBase
    {
        public GameCancelled() { }
        public GameCancelled(Guid guid) : base(guid) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubscribeToGameNotifications : GameMessageBase
    {
        public IActorRef Listener { get; private set; }

        public SubscribeToGameNotifications(Guid guid, IActorRef listener)
            : base(guid)
        {
            Listener = listener;
        }
    }

    /// <summary>
    /// The player has placed a stone onto the board.
    /// </summary>
    public class MoveMade : GameMessageBase
    {
        public Player Player { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public MoveMade() { }

        public MoveMade(Guid guid, Player player, int row, int column)
            : base(guid)
        {
            Player = player;
            Row = row;
            Column = column;
        }
    }

    /// <summary>
    /// The player places a stone.
    /// </summary>
    public class MakeMove : GameMessageBase
    {
        public Player Player { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MakeMove() { }

        public MakeMove(Guid guid, Player player, int row, int column)
            : base(guid)
        {
            Player = player;
            Row = row;
            Column = column;
        }
    }

    /// <summary>
    /// The response to the 'MakeMove' request.
    /// </summary>
    public class MoveResponse : GameMessageBase
    {
        public Player Player { get; set; }
        public Player NextPlayer { get; set; }
        public MoveStatus MoveStatus { get; set; }
        public GameStatus GameStatus { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public WinningLine WinningLine { get; set; }
        public IList<Tuple<int, int>> WinningLineCoords { get; set; }

        public MoveResponse() { }

        public MoveResponse(Guid guid,
            Player player, Player nextPlayer,
            int row, int column,
            MoveStatus moveStatus, GameStatus gameStatus, WinningLine winningLine) : base(guid)
        {
            Player = player;
            NextPlayer = nextPlayer;
            Row = row;
            Column = column;
            MoveStatus = moveStatus;
            GameStatus = gameStatus;
            WinningLine = winningLine;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StartThinking : GameMessageBase
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class StopThinking : GameMessageBase
    {

    }
}

