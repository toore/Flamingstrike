using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface IPlayer
    {
        string Name { get; }
        IEnumerable<Card> Cards { get; }
        void AddCard(Card card);
    }

    public class HumanPlayer : IPlayer
    {
        private readonly List<Card> _cards;

        public HumanPlayer(string name)
        {
            Name = name;
            _cards = new List<Card>();
        }

        public string Name { get; private set; }

        public IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
    }
}