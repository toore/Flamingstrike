using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class PlayerBuilder
    {
        private PlayerName _playerName = new GameEngine.PlayerBuilder().Build();

        public Player Build()
        {
            return new Player(_playerName, new List<ICard>());
        }

        public PlayerBuilder Player(PlayerName playerName)
        {
            _playerName = playerName;
            return this;
        }
    }
}