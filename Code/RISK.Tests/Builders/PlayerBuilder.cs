using RISK.Application;
using RISK.Application.Play;
using IPlayer = RISK.Application.IPlayer;

namespace RISK.Tests.Builders
{
    public class PlayerBuilder
    {
        private IPlayer _player;

        public InGameplayPlayer Build()
        {
            return new InGameplayPlayer(_player);
        }

        public PlayerBuilder PlayerId(IPlayer player)
        {
            _player = player;
            return this;
        }
    }
}