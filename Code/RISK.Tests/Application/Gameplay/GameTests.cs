using FluentAssertions;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameTests
    {
        private Game _game;
        private IWorldMap _worldMap;
        private ITurnFactory _turnFactory;
        private ITurn _nextTurn;
        private ITurn _turnAfterNextTurn;
        private IPlayers _players;
        private IPlayer _player1;
        private IPlayer _player2;
        private IAlternateGameSetup _alternateGameSetup;
        private IGameInitializerLocationSelector _gameInitializerLocationSelector;

        public GameTests()
        {
            _worldMap = Substitute.For<IWorldMap>();
            _turnFactory = Substitute.For<ITurnFactory>();
            _players = Substitute.For<IPlayers>();
            _alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            _gameInitializerLocationSelector = Substitute.For<IGameInitializerLocationSelector>();

            _nextTurn = Substitute.For<ITurn>();
            _turnAfterNextTurn = Substitute.For<ITurn>();
            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            _turnFactory.CreateSelectTurn(_player1, _worldMap).Returns(_nextTurn);
            _turnFactory.CreateSelectTurn(_player2, _worldMap).Returns(_turnAfterNextTurn);

            _players.GetAll().Returns(new[] { _player1, _player2 });

            _alternateGameSetup.Initialize(_gameInitializerLocationSelector).Returns(_worldMap);

            _game = new Game(_turnFactory, _players, _alternateGameSetup, _gameInitializerLocationSelector);
        }

        [Fact]
        public void Gets_world_map()
        {
            _game.GetWorldMap().Should().Be(_worldMap);
        }

        [Fact]
        public void Gets_next_turn()
        {
            var actual = _game.GetNextTurn();

            actual.Should().Be(_nextTurn);
        }

        [Fact]
        public void Gets_turn_after_next_turn()
        {
            _game.GetNextTurn();
            var actual = _game.GetNextTurn();

            actual.Should().Be(_turnAfterNextTurn);
        }
    }
}