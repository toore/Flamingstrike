using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Setup;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public class TerritoryConverterTests
    {
        [Fact]
        public void Initializes_territories()
        {
            var territory1 = Substitute.For<ITerritory>();
            var territory2 = Substitute.For<ITerritory>();
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();

            var sut = new TerritoryConverter();
            var actual = sut.Convert(new[]
            {
                new GameboardSetupTerritory(territory1, player1, 1),
                new GameboardSetupTerritory(territory2, player2, 2)
            });

            actual.ShouldAllBeEquivalentToInRisk(new[]
            {
                new GameboardTerritory(territory1, player1, 1),
                new GameboardTerritory(territory2, player2, 2),
            });
        }
    }
}