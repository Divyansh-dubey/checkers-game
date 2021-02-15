namespace Checkers.Api.Domain
{
    public class Move
    {
        public int StartIndex { get; private set; }
        public int TargetIndex { get; private set; }
        public int? JumpOverIndex { get; private set; }
        public bool IsJump => JumpOverIndex.HasValue;
        public Move(int startIndex, int targetIndex, int? jumpOverIndex = null)
        {
            StartIndex = startIndex;
            TargetIndex = targetIndex;
            JumpOverIndex = jumpOverIndex;
        }
    }
}
