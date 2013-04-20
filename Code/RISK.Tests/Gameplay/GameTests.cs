using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class GameTests
    {
        private Game _game;
        private IWorldMap _worldMap;
        private ITurnFactory _turnFactory;
        private ITurn _nextTurn;
        private IPlayerRepository _playerRepository;
        private IPlayer _player;
        private IAlternateGameSetup _alternateGameSetup;

        [SetUp]
        public void SetUp()
        {
            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _turnFactory = MockRepository.GenerateStub<ITurnFactory>();
            _playerRepository = MockRepository.GenerateStub<IPlayerRepository>();
            _alternateGameSetup = MockRepository.GenerateStub<IAlternateGameSetup>();

            _nextTurn = MockRepository.GenerateStub<ITurn>();
            _player = MockRepository.GenerateStub<IPlayer>();
            _turnFactory.Stub(x => x.Create(_player, _worldMap)).Return(_nextTurn);

            _playerRepository.Stub(x => x.GetAll()).Return(_player.AsList());

            _alternateGameSetup.Stub(x => x.Initialize()).Return(_worldMap);

            _game = new Game(_turnFactory, _playerRepository, _alternateGameSetup);
        }

        [Test]
        public void GetWorldMap_gets_world_map_instance()
        {
            _game.GetWorldMap().Should().Be(_worldMap);
        }

        [Test]
        public void GetNextTurn_creates_next_turn()
        {
            var actualNextTurn = _game.GetNextTurn();

            actualNextTurn.Should().Be(_nextTurn);
        }

        [Test]
        public void Initializes_with_alternate_territory_assignment()
        {
            _alternateGameSetup.AssertWasCalled(x => x.Initialize());
        }
    }
}