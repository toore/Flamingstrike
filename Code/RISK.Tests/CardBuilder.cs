using RISK.Application.Entities;

namespace RISK.Tests
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