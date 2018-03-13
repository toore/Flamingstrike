using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.FlamingStrike.GameEngine.Builders
{
    public class PlayerGameDataBuilder
    {
        private IPlayer _player = new PlayerBuilder().Build();

        public PlayerGameData Build()
        {
            return new PlayerGameData(_player, new List<ICard>());
        }

        public PlayerGameDataBuilder Player(IPlayer player)
        {
            _player = player;
            return this;
        }
    }
}