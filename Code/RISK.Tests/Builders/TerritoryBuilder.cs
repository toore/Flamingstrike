using NSubstitute;
using RISK.Application;
using RISK.Application.World;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Builders
{
    public class TerritoryBuilder
    {
        private ITerritoryId _territoryId = Substitute.For<ITerritoryId>();
        private IPlayerId _playerId = Substitute.For<IPlayerId>();

        public Territory Build()
        {
            return new Territory(_territoryId, _playerId, 0);
        }

        public TerritoryBuilder TerritoryId(ITerritoryId territoryId)
        {
            _territoryId = territoryId;
            return this;
        }

        public TerritoryBuilder Player(IPlayerId playerId)
        {
            _playerId = playerId;
            return this;
        }
    }
}