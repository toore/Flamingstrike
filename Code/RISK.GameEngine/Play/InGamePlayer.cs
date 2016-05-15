using System.Collections.Generic;
using System.Linq;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public interface IInGamePlayer
    {
        IPlayer Player { get; }
        IEnumerable<ICard> Cards { get; }
        void AddCard(ICard card);
        IEnumerable<ICard> AquireAllCards();
    }

    public class InGamePlayer : IInGamePlayer
    {
        private readonly IList<ICard> _cards = new List<ICard>();

        public InGamePlayer(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; }

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