using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public abstract class GameStateTestsBase
    {
        [Fact]
        public void Gets_current_player()
        {
            var currentPlayer = Substitute.For<IPlayer>();
            var gameData = Make.GameData
                .CurrentPlayer(currentPlayer)
                .Build();

            var sut = Create(gameData);

            sut.CurrentPlayer.Should().Be(currentPlayer);
        }

        protected abstract IGameState Create(GameData gameData);

        [Fact]
        public void Gets_territory()
        {
            var territory = Substitute.For<ITerritory>();
            var region = Substitute.For<IRegion>();
            territory.Region.Returns(region);
            var gameData = Make.GameData
                .WithTerritory(territory)
                .WithTerritory(Substitute.For<ITerritory>())
                .WithTerritory(Substitute.For<ITerritory>())
                .Build();

            var sut = Create(gameData);

            sut.GetTerritory(region).Should().Be(territory);
        }
    }
}