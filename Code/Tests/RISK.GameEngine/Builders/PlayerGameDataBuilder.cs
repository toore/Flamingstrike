using System.Collections.Generic;
using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace Tests.RISK.GameEngine.Builders
{
    public class PlayerGameDataBuilder
    {
        private IPlayer _player = Make.Player.Build();

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