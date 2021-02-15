using System;

namespace Checkers.Api.Domain
{
    public class Player
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public PawnColor Color { get; private set; }

        public static Player Create(Guid id, string name, PawnColor color)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("New name can not be null", nameof(name));
            }
            return new Player
            {
                Id = id,
                Name = name,
                Color = color
            };
        }
    }
}
