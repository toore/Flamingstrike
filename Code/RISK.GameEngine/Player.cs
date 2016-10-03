﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine
{
    public interface IPlayer
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

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<ICard> Cards => _cards;

        public int ArmiesToPlace { get; private set; }

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
            if (!HasArmiesLeftToPlace())
            {
                throw new InvalidOperationException("No armies left to place");
            }
            territory.Armies++;
            ArmiesToPlace--;
        }

        public void SetArmiesToPlace(int armiesToPlace)
        {
            if (armiesToPlace < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(armiesToPlace), armiesToPlace, "value must be greater than or equal to zero");
            }
            ArmiesToPlace = armiesToPlace;
        }
    }
}