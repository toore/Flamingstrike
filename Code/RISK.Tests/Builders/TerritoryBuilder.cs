using NSubstitute;
using RISK.Application;
using RISK.Application.World;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Builders
{
    public class TerritoryBuilder
    {
        private IPlayerId _playerId = Substitute.For<IPlayerId>();

        public Territory Build()
        {
            return new Territory(Substitute.For<ITerritoryId>(), _playerId, 0);
        }

        public TerritoryBuilder Player(IPlayerId playerId)
        {
            _playerId = playerId;
            return this;
        }
    }
}