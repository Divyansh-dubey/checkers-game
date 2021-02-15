namespace Checkers.Api.Domain
{
    public class Field
    {
        public int Index { get; private set; }
        public bool Empty => Pawn is null;
        public bool IsUsable { get; internal set; }
        public Pawn Pawn { get; internal set; }
        public static bool IsEmpty(Field field) => field == null || field.Empty;

        public static Field Create(bool isUsable, int index)
        {
            return new Field
            {
                Index = index,
                IsUsable = isUsable
            };
        }

        public void Clear()
        {
            Pawn = null;
        }

        public void PutPawn(Pawn pawn)
        {
            if(!IsUsable)
            {
                throw new InvalidMoveException("Cannot put pawn on a field that is not usable");
            }
            Pawn = pawn;
        }
    }
}