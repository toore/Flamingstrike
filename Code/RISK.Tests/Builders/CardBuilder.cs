using RISK.Application.Play;

namespace RISK.Tests.Builders
{
    public class CardBuilder
    {
        private const CardSymbol _cardSymbol = CardSymbol.Infantry;

        public Card Build()
        {
            return new Card(_cardSymbol);
        }
    }
}