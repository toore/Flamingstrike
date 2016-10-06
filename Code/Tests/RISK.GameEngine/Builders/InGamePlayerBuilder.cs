using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace Tests.RISK.GameEngine.Builders
{
    public class InGamePlayerBuilder
    {
        private readonly IPlayer _player = Make.Player.Build();

        public PlayerGameData Build()
        {
            return new PlayerGameData(_player);
        }
    }
}