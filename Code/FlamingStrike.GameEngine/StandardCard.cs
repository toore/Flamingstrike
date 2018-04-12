namespace FlamingStrike.GameEngine
{
    public class StandardCard : ICard
    {
        public StandardCard(Region region, CardSymbol cardSymbol)
        {
            Region = region;
            CardSymbol = cardSymbol;
        }

        public Region Region { get; }
        public CardSymbol CardSymbol { get; }
    }
}