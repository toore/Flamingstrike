using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class PlayerGameDataBuilder
    {
        private PlayerName _playerName = new PlayerBuilder().Build();

        public PlayerGameData Build()
        {
            return new PlayerGameData(_playerName, new List<ICard>());
        }

        public PlayerGameDataBuilder Player(PlayerName playerName)
        {
            _playerName = playerName;
            return this;
        }
    }
}