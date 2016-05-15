using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public interface IDeck
    {
        ICard Draw();
        void ReturnToBottom(IEnumerable<ICard> cards);
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

        public void ReturnToBottom(IEnumerable<ICard> cards)
        {
            foreach (var card in cards)
            {
                _cards.Enqueue(card);
            }
        }
    }
}