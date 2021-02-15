using System.Collections.Generic;

namespace Checkers.Api.Repositories.Models
{
    public class BoardDbModel
    {
        public int Size { get; set; }
        public bool LongMoveAllowed { get; set; }
        public string CurrentTurn { get; set; }
        public List<PawnDbModel> Pawns { get; set; }
    }

    public class PawnDbModel
    {
        public int Index { get; set; }
        public string Color { get; set; }
        public bool IsKing { get; set; }
    }
}
