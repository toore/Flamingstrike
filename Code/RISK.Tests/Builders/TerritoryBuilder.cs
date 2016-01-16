using NSubstitute;
using RISK.Application;
using RISK.Application.World;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Builders
{
    public class TerritoryBuilder
    {
        private ITerritoryId _territoryId = Substitute.For<ITerritoryId>();
        private IPlayer _player = Substitute.For<IPlayer>();

        public Territory Build()
        {
            return new Territory(_territoryId, _player, 0);
        }

        public TerritoryBuilder TerritoryId(ITerritoryId territoryId)
        {
            _territoryId = territoryId;
            return this;
        }

        public TerritoryBuilder Player(IPlayer player)
        {
            _player = player;
            return this;
        }
    }
}