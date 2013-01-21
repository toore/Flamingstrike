namespace RISK.Domain.Entities
{
    public class HumanPlayer : IPlayer
    {
        public HumanPlayer(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}