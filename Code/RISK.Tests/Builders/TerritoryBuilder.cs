using NSubstitute;
using RISK.Application;
using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class TerritoryBuilder
    {
        private IRegion _region = Substitute.For<IRegion>();
        private IPlayer _player = Substitute.For<IPlayer>();
        private int _armies;

        public Territory Build()
        {
            return new Territory(_region, _player, _armies);
        }

        public TerritoryBuilder TerritoryGeography(IRegion region)
        {
            _region = region;
            return this;
        }

        public TerritoryBuilder Player(IPlayer player)
        {
            _player = player;
            return this;
        }

        public TerritoryBuilder Armies(int armies)
        {
            _armies = armies;
            return this;
        }
    }
}