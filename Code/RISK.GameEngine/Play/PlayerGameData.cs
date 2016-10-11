using System;
using System.Collections.Generic;

namespace RISK.GameEngine.Play
{
    public interface IPlayerGameData
    {
        IPlayer Player { get; }
        IReadOnlyList<ICard> Cards { get; }
    }

    public class PlayerGameData : IPlayerGameData
    {
        public PlayerGameData(IPlayer player, IReadOnlyList<ICard> cards)
        {
            Player = player;
            Cards = cards;
        }

        public IPlayer Player { get; }
        public IReadOnlyList<ICard> Cards { get; }

        public void AddCard(ICard card)
        {
            throw new NotImplementedException();

            //_cards.Add(card);
        }

        public IEnumerable<ICard> AquireAllCards()
        {
            throw new NotImplementedException();
            //var cardsToReturn = _cards.ToList();
            //_cards.Clear();

            //return cardsToReturn;
        }
    }
}