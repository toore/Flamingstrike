using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class GameboardTerritoryBuilder
    {
        private IPlayer _player = Substitute.For<IPlayer>();

        public GameboardTerritory Build()
        {
            return new GameboardTerritory(Substitute.For<ITerritory>(), _player, 0);
        }

        public GameboardTerritoryBuilder Player(IPlayer player)
        {
            _player = player;
            return this;
        }
    }
}