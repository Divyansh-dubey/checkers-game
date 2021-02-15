using System.Collections.Generic;

namespace Checkers.Api.Handlers.Common
{
    public class BoardData
    {
        public int Size { get; set; }
        public IEnumerable<FieldData> Fields { get; set; }
        public IEnumerable<MoveData> AvailableMoves { get; set; }
    }

    public class FieldData
    {
        public int Index { get; set; }
        public bool IsUsable { get; set; }
        public string PawnColor { get; set; }
        public bool IsKing { get; set; }
    }

    public class MoveData
    {
        public int StartIndex { get; set; }
        public int TargetIndex { get; set; }
        public int? JumpOverIndex { get; set; }
    }
}
