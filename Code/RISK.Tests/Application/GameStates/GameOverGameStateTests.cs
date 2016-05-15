using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public class GameOverGameStateTests
    {
        //[Fact]
        //public void Is_game_over_when_all_territories_belongs_to_one_player()
        //{
        //    var player = Substitute.For<IPlayer>();
        //    var gamePlaySetup = Make.GamePlaySetup
        //        .WithTerritory(Make.Territory.Player(player).Build())
        //        .WithTerritory(Make.Territory.Player(player).Build())
        //        .Build();

        //    var sut = Create(gamePlaySetup);

        //    sut.IsGameOver().Should().BeTrue();
        //}

        //[Fact]
        //public void Is_not_game_over_when_more_than_one_player_occupies_territories()
        //{
        //    var gamePlaySetup = Make.GamePlaySetup
        //        .WithTerritory(Make.Territory.Build())
        //        .WithTerritory(Make.Territory.Build())
        //        .Build();

        //    var sut = Create(gamePlaySetup);

        //    sut.IsGameOver().Should().BeFalse();
        //}

        private IGameState Create(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }

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
    }
}