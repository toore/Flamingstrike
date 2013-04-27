using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class CardFactory : ICardFactory
    {
        public Card Create()
        {
            return new Card(CardSymbol.Artillery);
        }
    }
}