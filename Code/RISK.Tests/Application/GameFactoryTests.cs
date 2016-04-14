using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Application.Play.Planning;
using RISK.Tests.Builders;
using RISK.Tests.Extensions;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameFactoryTests
    {
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IGameStateFactory _gameStateFactory;
        private IDeckFactory _deckFactory;
        private GameFactory _gameFactory;
        private readonly GameFactory _sut;

        public GameFactoryTests()
        {
            _armyDraftCalculator = Substitute.For<IArmyDraftCalculator>();
            _gameStateFactory = Substitute.For<IGameStateFactory>();
            _deckFactory = Substitute.For<IDeckFactory>();

            _gameFactory = _sut = new GameFactory(_gameStateFactory, _armyDraftCalculator, _deckFactory);
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
            _armyDraftCalculator.Calculate(firstPlayer, Argx.IsEquivalent(territory, anotherTerritory)).Returns(draftedArmies);
            _gameStateFactory
                .CreateDraftArmiesGameState(Arg.Is<GameData>(x =>
                    x.CurrentPlayer == firstPlayer
                    &&
                    x.Players.IsEquivalent(firstPlayer, secondPlayer, thirdPlayer)
                    &&
                    x.Territories.IsEquivalent(territory, anotherTerritory)),
                    draftedArmies)
                .Returns(draftArmiesGameState);

            var game = _sut.Create(gamePlaySetup);

            game.CurrentPlayer.Should().Be(expectedCurrentPlayer);
        }
    }
}