using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Tests.Builders;
using RISK.Tests.Extensions;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameFactoryTests
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IDeckFactory _deckFactory;
        private readonly GameFactory _sut;

        public GameFactoryTests()
        {
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _deckFactory = Substitute.For<IDeckFactory>();

            _sut = new GameFactory(_gameStateConductor, _deckFactory);
        }

        [Theory]
        [AutoData]
        public void Game_initializes_draft_armies_game_state(int draftedArmies)
        {
            var territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();
            var firstPlayer = Substitute.For<IPlayer>();
            var secondPlayer = Substitute.For<IPlayer>();
            var thirdPlayer = Substitute.For<IPlayer>();
            var gamePlaySetup = Make.GamePlaySetup
                .WithTerritory(territory)
                .WithTerritory(anotherTerritory)
                .WithPlayer(firstPlayer)
                .WithPlayer(secondPlayer)
                .WithPlayer(thirdPlayer)
                .Build();
            var draftArmiesGameState = Substitute.For<IGameState>();
            var expectedCurrentPlayer = Substitute.For<IPlayer>();
            draftArmiesGameState.CurrentPlayer.Returns(expectedCurrentPlayer);
            _gameStateConductor
                .InitializeFirstPlayerTurn(Arg.Is<GameData>(x =>
                    x.CurrentPlayer == firstPlayer
                    &&
                    x.Players.IsEquivalent(firstPlayer, secondPlayer, thirdPlayer)
                    &&
                    x.Territories.IsEquivalent(territory, anotherTerritory)))
                .Returns(draftArmiesGameState);

            var game = _sut.Create(gamePlaySetup);

            game.CurrentPlayer.Should().Be(expectedCurrentPlayer);
        }
    }
}