using RISK.Application.Play;
using RISK.Application.Play.GamePhases;

namespace RISK.Tests.Application.GameStates
{
    public class GameOverGameStateTests : GameStateTestsBase
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

        protected override IGameState Create(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }
    }
}