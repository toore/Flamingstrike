using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameStateTests
    {
        [Fact]
        public void Gets_attack_candidate()
        {
            var currentPlayer = Substitute.For<IPlayer>();
            var otherPlayer = Substitute.For<IPlayer>();
            var attackingTerritory = Substitute.For<ITerritory>();
            var attackeeTerritory = Substitute.For<ITerritory>();
            attackingTerritory.HasBorderTo(attackeeTerritory).Returns(true);
            var gameboardTerritories = new[]
            {
                new GameboardTerritory(attackingTerritory, currentPlayer, 1),
                new GameboardTerritory(attackeeTerritory, otherPlayer, 1)
            };
            var sut = new GameState
            {
                CurrentPlayer = currentPlayer,
                Territories = gameboardTerritories
            };

            sut.GetAttackCandidates(attackingTerritory).Should().BeEquivalentTo(attackeeTerritory);
        }
    }
}