using FluentAssertions;
using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using RISK.GameEngine.Setup;
using Tests.RISK.GameEngine.Builders;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace Tests.RISK.GameEngine.Play
{
    public class GameTests
    {
        private readonly GameFactory _factory;
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;
        private readonly IGameObserver _gameObserver;

        public GameTests()
        {
            _gameStateFactory = Substitute.For<IGameStateFactory>();
            _armyDraftCalculator = Substitute.For<IArmyDraftCalculator>();
            _deckFactory = Substitute.For<IDeckFactory>();

            _factory = new GameFactory(_gameStateFactory, _armyDraftCalculator, _deckFactory);

            _gameObserver = Substitute.For<IGameObserver>();
        }

        [Fact]
        public void Current_game_player_data_belongs_to_first_player()
        {
            var firstPlayer = Substitute.For<IPlayer>();
            var gamePlaySetup = Make.GamePlaySetup
                .WithPlayer(firstPlayer)
                .WithPlayer(Substitute.For<IPlayer>()).Build();

            var sut = Create(gamePlaySetup);

            sut.CurrentPlayerGameData.Player.Should().Be(firstPlayer);
        }

        [Fact]
        public void Gets_territories()
        {
            var territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();
            var aThirdTerritory = Substitute.For<ITerritory>();
            var gamePlaySetup = Make.GamePlaySetup
                .WithTerritory(territory)
                .WithTerritory(anotherTerritory)
                .WithTerritory(aThirdTerritory).Build();

            var sut = Create(gamePlaySetup);

            sut.Territories.Should().BeEquivalentTo(territory, anotherTerritory, aThirdTerritory);
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _factory.Create(_gameObserver, gamePlaySetup);
        }
    }
}