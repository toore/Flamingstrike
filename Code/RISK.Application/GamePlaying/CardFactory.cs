using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public class CardFactory : ICardFactory
    {
        public Card Create()
        {
            return new Card(CardSymbol.Artillery);
        }
    }
}