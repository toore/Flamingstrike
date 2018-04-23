using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class PlayerBuilder
    {
        private PlayerName _name = new GameEngine.PlayerBuilder().Build();
        private readonly List<ICard> _cards = new List<ICard>();

        public Player Build()
        {
            return new Player(_name, _cards);
        }

        public PlayerBuilder Name(PlayerName playerName)
        {
            _name = playerName;
            return this;
        }

        public PlayerBuilder AddCard(ICard card)
        {
            _cards.Add(card);
            return this;
        }
    }
}