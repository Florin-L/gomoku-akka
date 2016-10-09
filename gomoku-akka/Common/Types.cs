using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

namespace Gomoku.Common
{
    /// <summary>
    /// 
    /// </summary>
    public enum PlayerColor
    {
        Black,
        White
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PlayerType
    {
        Human,
        Computer
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GameStatus
    {
        Continue,
        WhiteWon,
        BlackWon,
        Draw,
        Error
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MoveStatus
    {
        Accepted,
        Rejected
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WinningLine
    {
        None,
        Horiz,
        DownLeft,
        DownRight,
        Vert
    }

    /// <summary>
    /// 
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public PlayerColor Color { get; set; }
        public PlayerType Type { get; set; }

        public char Symbol
        {
            get
            {
                return (Color == PlayerColor.White) ? 'w' : 'b';
            }
        }

        public Player() { }

        public Player(string name, PlayerType type, PlayerColor color)
        {
            this.Name = name;
            this.Type = type;
            this.Color = color;
        }

        public bool IsComputer()
        {
            return this.Type == PlayerType.Computer;
        }

        public bool IsHuman()
        {
            return this.Type == PlayerType.Human;
        }

        public override bool Equals(object obj)
        {
            var p = obj as Player;

            if (p == null)
            {
                return false;
            }

            return p.Name.Equals(this.Name) && (p.Color == this.Color) && (p.Type == this.Type);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}", Name, Symbol, Type);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class HumanPlayer : Player
    {
        public HumanPlayer() { }

        public HumanPlayer(string name, PlayerColor color)
            : base(name, PlayerType.Human, color)
        { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ComputerPlayer : Player
    {
        public ComputerPlayer() { }

        public ComputerPlayer(string name, PlayerColor color)
            : base(name, PlayerType.Computer, color)
        {
        }
    }
}
