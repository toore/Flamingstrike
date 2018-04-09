namespace FlamingStrike.GameEngine
{
    public class StandardCard : ICard
    {
        public StandardCard(IRegion region, CardSymbol cardSymbol)
        {
            Region = region;
            CardSymbol = cardSymbol;
        }

        public IRegion Region { get; }
        public CardSymbol CardSymbol { get; }
    }
}