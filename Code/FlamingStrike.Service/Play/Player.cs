namespace FlamingStrike.Service.Play
{
    public class Player
    {
        public string Name { get; }
        public int NumberOfCards { get; }

        public Player(string name, int numberOfCards)
        {
            Name = name;
            NumberOfCards = numberOfCards;
        }
    }
}