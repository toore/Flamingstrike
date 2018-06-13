namespace FlamingStrike.Service.Play
{
    public class PlayerDto
    {
        public string Name { get; }
        public int NumberOfCards { get; }

        public PlayerDto(string name, int numberOfCards)
        {
            Name = name;
            NumberOfCards = numberOfCards;
        }
    }
}