using System.Collections.Generic;
using System.Linq;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public interface IPlayer : Core.IPlayer
    {
        string Name { get; }
        IEnumerable<ICard> Cards { get; }
        int ArmiesToPlace { get; }
        bool HasArmiesLeftToPlace();
        void AddCard(ICard card);
        IEnumerable<ICard> AquireAllCards();
        void PlaceArmy(Territory territory);
        void SetArmiesToPlace(int armiesToPlace);
    }

    public class Player : IPlayer
    {
        private readonly IList<ICard> _cards = new List<ICard>();
        private int _armiesToPlace = 0;

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<ICard> Cards => _cards;

        public int ArmiesToPlace => _armiesToPlace;

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }

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

        public void PlaceArmy(Territory territory)
        {
            territory.Armies++;
            _armiesToPlace--;
        }

        public void SetArmiesToPlace(int armiesToPlace)
        {
            _armiesToPlace = armiesToPlace;
        }
    }
}