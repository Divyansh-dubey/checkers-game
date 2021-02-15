using System;
using System.Linq;

namespace Checkers.Api.Domain
{
    public class Pawn
    {
        public PawnColor Color { get; private set; }
        public bool IsKing { get; private set; }
        public Pawn(PawnColor color, bool isKing = false)
        {
            Color = color;
            IsKing = isKing;
        }

        public void MakeKing()
        {
            IsKing = true;
        }
    }

    public class PawnColor
    {
        private const string WHITE = "white";
        private const string BLACK = "black";
        public int Number { get; private set; }
        public string Name { get; private set; }

        public static PawnColor White { get; } = new PawnColor { Name = WHITE, Number = 0 };
        public static PawnColor Black { get; } = new PawnColor { Name = BLACK, Number = 1 };

        public static PawnColor FromName(string name)
        {
            if (!new string[] { WHITE, BLACK }.Contains(name))
            {
                throw new ArgumentException("Unknown color name", nameof(name));
            }
            return name == WHITE ? White : Black;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(PawnColor))
            {
                throw new ArgumentException("Pawn color cannot be compared with other data structures", nameof(obj));
            }
            return (obj as PawnColor).Number == Number;
        }
        public override int GetHashCode() => base.GetHashCode();
    }
}
