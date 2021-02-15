namespace Checkers.Api.Settings
{
    public class CheckersDbSettings : ICheckersDbSettings
    {
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICheckersDbSettings
    {
        string GamesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}