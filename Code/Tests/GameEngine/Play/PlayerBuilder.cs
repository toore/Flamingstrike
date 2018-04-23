using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class PlayerBuilder
    {
        private PlayerName _playerName = new GameEngine.PlayerBuilder().Build();
        private readonly List<ICard> _cards = new List<ICard>();

        public Player Build()
        {
            return new Player(_playerName, _cards);
        }

        public PlayerBuilder Player(PlayerName playerName)
        {
            _playerName = playerName;
            return this;
        }

        public PlayerBuilder AddCard(ICard card)
        {
            _cards.Add(card);
            return this;
        }
    }
}