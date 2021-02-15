using System;

namespace Checkers.Api.Handlers.Common
{
    public class GameData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class WinnerData
    {
        public Guid Id { get; set; }
    }
}
