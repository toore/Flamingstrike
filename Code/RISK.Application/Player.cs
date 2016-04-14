using System.Collections.Generic;
using RISK.Application.Play;

namespace RISK.Application
{
    public interface IPlayer
    {
        string Name { get; }
        IEnumerable<ICard> Cards { get; }
        void AddCard(ICard card);
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
    }
}