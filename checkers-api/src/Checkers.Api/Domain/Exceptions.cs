using System;

namespace Checkers.Api.Domain
{
    public class CheckersException : Exception
    {
        public CheckersException(string message) : base(message) { }
    }

    public class InvalidMoveException : CheckersException 
    {
        public InvalidMoveException(string message) : base(message) { }
    }

    public class InvalidGameOperation : CheckersException
    {
        public InvalidGameOperation(string message) : base(message) { }
    }
}
