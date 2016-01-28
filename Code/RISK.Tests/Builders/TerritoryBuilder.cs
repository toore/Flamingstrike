using NSubstitute;
using RISK.Application;
using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class TerritoryBuilder
    {
        private ITerritoryGeography _territoryGeography = Substitute.For<ITerritoryGeography>();
        private IPlayer _player = Substitute.For<IPlayer>();
        private int _armies;

        public Territory Build()
        {
            return new Territory(_territoryGeography, _player, _armies);
        }

        public TerritoryBuilder TerritoryGeography(ITerritoryGeography territoryGeography)
        {
            _territoryGeography = territoryGeography;
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