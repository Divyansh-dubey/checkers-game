using System;

namespace Checkers.Api.Repositories.Models
{
    public class GameDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? WhitePlayerId { get; set; }
        public string WhitePlayerName { get; set; }
        public Guid? BlackPlayerId { get; set; }
        public string BlackPlayerName { get; set; }
        public bool HasStarted { get; set; }
        public BoardDbModel Board { get; set; }
    }
}
