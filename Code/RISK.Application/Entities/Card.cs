namespace RISK.Application.Entities
{
    public class Card
    {
        public Card(CardSymbol symbol)
        {
            Symbol = symbol;
        }

        public CardSymbol Symbol { get; private set; }
    }

    public enum CardSymbol
    {
        Infantry, Cavalry, Artillery
    }
}