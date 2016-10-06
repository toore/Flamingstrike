using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Play.GameStates
{
    public class PlayerInPlay
    {
        private readonly IList<ICard> _cards = new List<ICard>();

        public PlayerInPlay(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; private set; }

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