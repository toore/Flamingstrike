namespace RISK.Application
{
    public interface IPlayerId
    {
        string Name { get; }
    }

    public class PlayerId : IPlayerId
    {
        public PlayerId(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}