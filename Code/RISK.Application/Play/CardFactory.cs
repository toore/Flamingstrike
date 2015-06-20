namespace RISK.Application.Play
{
    public interface ICardFactory
    {
        Card Create();
    }

    public class CardFactory : ICardFactory
    {
        public Card Create()
        {
            return new Card(CardSymbol.Artillery);
        }
    }
}