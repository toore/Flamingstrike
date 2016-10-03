namespace RISK.GameEngine
{
    public class StandardCard : ICard
    {
        public StandardCard(IRegion region, CardSymbol cardSymbol)
        {
            Region = region;
            CardSymbol = cardSymbol;
        }

        public IRegion Region { get; private set; }
        public CardSymbol CardSymbol { get; private set; }
    }
}