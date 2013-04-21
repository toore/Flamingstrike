using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class GameTests
    {
        private Game _game;
        private IWorldMap _worldMap;
        private ITurnFactory _turnFactory;
        private ITurn _nextTurn;
        private ITurn _turnAfterNextTurn;
        private IPlayerRepository _playerRepository;
        private IPlayer _player1;
        private IPlayer _player2;
        private IAlternateGameSetup _alternateGameSetup;

        [SetUp]
        public void SetUp()
        {
            _worldMap = Substitute.For<IWorldMap>();
            _turnFactory = Substitute.For<ITurnFactory>();
            _playerRepository = Substitute.For<IPlayerRepository>();
            _alternateGameSetup = Substitute.For<IAlternateGameSetup>();

            _nextTurn = Substitute.For<ITurn>();
            _turnAfterNextTurn = Substitute.For<ITurn>();
            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            _turnFactory.Create(_player1, _worldMap).Returns(_nextTurn);
            _turnFactory.Create(_player2, _worldMap).Returns(_turnAfterNextTurn);

            _playerRepository.GetAll().Returns(new[] { _player1, _player2 });

            _alternateGameSetup.Initialize().Returns(_worldMap);

            _game = new Game(_turnFactory, _playerRepository, _alternateGameSetup);
        }

        [Test]
        public void Gets_world_map()
        {
            _game.GetWorldMap().Should().Be(_worldMap);
        }

        [Test]
        public void Gets_next_turn()
        {
            var actual = _game.GetNextTurn();

            actual.Should().Be(_nextTurn);
        }

        [Test]
        public void Gets_turn_after_next_turn()
        {
            _game.GetNextTurn();
            var actual = _game.GetNextTurn();

            actual.Should().Be(_turnAfterNextTurn);
        }
    }
}