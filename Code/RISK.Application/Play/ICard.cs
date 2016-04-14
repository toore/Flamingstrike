using RISK.Application.World;

namespace RISK.Application.Play
{
    public enum CardSymbol
    {
        Infantry,
        Cavalry,
        Artillery
    }

    public interface ICard {}

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

    public class WildCard : ICard
    {
        
    }
}