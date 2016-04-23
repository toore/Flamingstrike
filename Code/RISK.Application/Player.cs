using System.Collections.Generic;
using System.Linq;
using RISK.Application.Play;

namespace RISK.Application
{
    public interface IPlayer
    {
        string Name { get; }
        IEnumerable<ICard> Cards { get; }
        void AddCard(ICard card);
        IEnumerable<ICard> AquireAllCards();
    }

    public class Player : IPlayer
    {
        private readonly IList<ICard> _cards = new List<ICard>();

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<ICard> Cards => _cards;

        public void AddCard(ICard card)
        {
            _cards.Add(card);
        }

        public IEnumerable<ICard> AquireAllCards()
        {
            var cardsToReturn = _cards.ToList();
            _cards.Clear();

            return cardsToReturn;
        }
    }
}