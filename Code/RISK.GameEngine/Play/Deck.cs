using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Play
{
    public interface IDeck
    {
        CardDrawnAndRestOfDeck DrawCard();
    }

    public class Deck : IDeck
    {
        private readonly IReadOnlyList<ICard> _cards;

        public Deck(IReadOnlyList<ICard> cards)
        {
            _cards = cards;
        }

        public CardDrawnAndRestOfDeck DrawCard()
        {
            var topCard = _cards.First();
            var restOfDeck = new Deck(_cards.Skip(1).ToList());

            return new CardDrawnAndRestOfDeck(topCard, restOfDeck);
        }
    }

    public class CardDrawnAndRestOfDeck
    {
        public ICard CardDrawn { get; }
        public IDeck RestOfDeck { get; }

        public CardDrawnAndRestOfDeck(ICard cardDrawn, IDeck restOfDeck)
        {
            CardDrawn = cardDrawn;
            RestOfDeck = restOfDeck;
        }
    }
}