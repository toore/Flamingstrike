using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IPlayer
    {
        PlayerName Name { get; }
        IReadOnlyList<ICard> Cards { get; }
        void EliminatedBy(IPlayer attackingPlayer);
        void AddCards(IEnumerable<ICard> cards);
        void AddCard(IDeck deck);
    }

    public class Player : IPlayer
    {
        private readonly List<ICard> _cards;

        public Player(PlayerName name, List<ICard> cards)
        {
            _cards = cards;
            Name = name;
        }

        public PlayerName Name { get; }

        public IReadOnlyList<ICard> Cards => _cards.ToList();

        public bool IsEliminated { get; private set; }

        public void EliminatedBy(IPlayer attackingPlayer)
        {
            if (IsEliminated)
            {
                throw new InvalidOperationException("Player is already eliminated");
            }

            attackingPlayer.AddCards(_cards);
            _cards.Clear();
            IsEliminated = true;
        }

        public void AddCards(IEnumerable<ICard> cards)
        {
            _cards.AddRange(cards);
        }

        public void AddCard(IDeck deck)
        {
            _cards.Add(deck.DrawCard());
        }
    }
}