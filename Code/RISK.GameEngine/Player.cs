namespace RISK.GameEngine
{
    public interface IPlayer
    {
        string Name { get; }
    }

    public class Player : IPlayer
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}