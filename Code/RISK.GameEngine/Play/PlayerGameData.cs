using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Play
{
    public interface IPlayerGameData
    {
        IPlayer Player { get; }
        IEnumerable<ICard> Cards { get; }
    }

    public class PlayerGameData : IPlayerGameData
    {
        private readonly IList<ICard> _cards = new List<ICard>();

        public PlayerGameData(IPlayer player)
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