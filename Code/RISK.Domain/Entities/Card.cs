namespace RISK.Domain.Entities
{
    public class Card
    {
        public Card(CardSymbol symbol)
        {
            Symbol = symbol;
        }

        public CardSymbol Symbol { get; private set; }
    }
}