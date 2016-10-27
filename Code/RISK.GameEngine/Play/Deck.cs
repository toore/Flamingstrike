using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Play
{
    public interface IDeck
    {
        DrawCard DrawCard();
    }

    public class Deck : IDeck
    {
        private readonly IReadOnlyList<ICard> _cards;

        public Deck(IReadOnlyList<ICard> cards)
        {
            _cards = cards;
        }

        public DrawCard DrawCard()
        {
            var topCard = _cards.First();
            var restOfDeck = new Deck(_cards.Skip(1).ToList());

            return new DrawCard(topCard, restOfDeck);
        }
    }

    public class DrawCard
    {
        public ICard TopCard { get; }
        public IDeck RestOfTheDeck { get; }

        public DrawCard(ICard topCard, IDeck restOfTheDeck)
        {
            TopCard = topCard;
            RestOfTheDeck = restOfTheDeck;
        }
    }
}