using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Application.Gameplay
{
    static class TurnTestsExtensions
    {
        public static void IsConnectedTo(this ILocation from, params ILocation[] to)
        {
            from.Connections.Returns(to);
        }

        public static void AttackerAlwaysWins(this IBattleCalculator battleCalculator, ITerritory from, ITerritory to)
        {
            battleCalculator
                .When(x => x.Attack(from, to))
                .Do(x => to.Occupant = from.Occupant);
        }

        public static ITerritory HasTerritory(this IWorldMap worldMap, ILocation location, IPlayer owner)
        {
            var territory = Substitute.For<ITerritory>();
            territory.Location.Returns(location);
            territory.Occupant = owner;

            worldMap.GetTerritory(location).Returns(territory);

            return territory;
        }
    }
}