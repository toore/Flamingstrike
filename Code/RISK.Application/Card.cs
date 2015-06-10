namespace RISK.Application
{
    public enum CardSymbol
    {
        Infantry, Cavalry, Artillery
    }

    public class Card
    {
        public Card(CardSymbol symbol)
        {
            Symbol = symbol;
        }

        public CardSymbol Symbol { get; private set; }
    }
}