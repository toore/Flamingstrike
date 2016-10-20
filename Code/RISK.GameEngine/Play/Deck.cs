using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Play
{
    public interface IDeck
    {
        ICard Draw();
        IReadOnlyList<ICard> Cards();
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

        public IReadOnlyList<ICard> Cards()
        {
            return _cards.ToList();
        }
    }
}