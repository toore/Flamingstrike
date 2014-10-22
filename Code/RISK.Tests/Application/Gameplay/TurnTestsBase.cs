using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnTestsBase
    {
        protected ITerritory StubTerritory(IWorldMap worldMap, ILocation location, IPlayer owner)
        {
            var territory = Substitute.For<ITerritory>();
            territory.Location.Returns(location);
            territory.Occupant = owner;

            worldMap.GetTerritory(location).Returns(territory);

            return territory;
        }
    }
}