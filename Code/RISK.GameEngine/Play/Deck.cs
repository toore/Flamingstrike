using System.Collections.Generic;

namespace RISK.GameEngine.Play
{
    public interface IDeck
    {
        ICard Draw();
    }

    public class Deck : IDeck
    {
        private readonly Queue<ICard> _cards;

        public Deck(IEnumerable<ICard> cards)
        {
            _cards = new Queue<ICard>(cards);
        }

        public ICard Draw()
        {
            var card = _cards.Dequeue();
            return card;
        }
    }
}