using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IDeck
    {
        ICard DrawCard();
    }

    public class Deck : IDeck
    {
        private readonly Stack<ICard> _cards;

        public Deck(Stack<ICard> cards)
        {
            _cards = cards;
        }

        public ICard DrawCard()
        {
            return _cards.Pop();
        }
    }
}