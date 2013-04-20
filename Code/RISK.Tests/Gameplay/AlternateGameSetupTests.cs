using System.Linq;
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
    public class AlternateGameSetupTests
    {
        private AlternateGameSetup _alternateGameSetup;
        private IPlayerRepository _playerRepository;
        private ILocationProvider _locationProvider;
        private ILocation _location1;
        private ILocation _location2;
        private ILocation _location3;
        private IPlayer _player1;
        private IPlayer _player2;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private ITerritory _territory3;
        private IRandomOrderer _randomOrderer;

        [SetUp]
        public void SetUp()
        {
            _playerRepository = MockRepository.GenerateStub<IPlayerRepository>();
            _locationProvider = MockRepository.GenerateStub<ILocationProvider>();
            _randomOrderer = MockRepository.GenerateStub<IRandomOrderer>();

            _player1 = MockRepository.GenerateStub<IPlayer>();
            _player2 = MockRepository.GenerateStub<IPlayer>();
            var playersInRepository = Enumerable.Empty<IPlayer>().ToList();
            _playerRepository
                .Stub(x => x.GetAll())
                .Return(playersInRepository);

            _location1 = MockRepository.GenerateStub<ILocation>();
            _location2 = MockRepository.GenerateStub<ILocation>();
            _location3 = MockRepository.GenerateStub<ILocation>();
            var locations = new[] { _location1, _location2, _location3 };
            _locationProvider
                .Stub(x => x.GetAll())
                .Return(locations);

            _alternateGameSetup = new AlternateGameSetup(_playerRepository, _locationProvider, _randomOrderer);

            //_worldMap = MockRepository.GenerateStub<IWorldMap>();
            _territory1 = MockRepository.GenerateStub<ITerritory>();
            _territory2 = MockRepository.GenerateStub<ITerritory>();
            _territory3 = MockRepository.GenerateStub<ITerritory>();
            //_worldMap.Stub(x => x.GetTerritory(_location1)).Return(_territory1);
            //_worldMap.Stub(x => x.GetTerritory(_location2)).Return(_territory2);
            //_worldMap.Stub(x => x.GetTerritory(_location3)).Return(_territory3);

            _randomOrderer.Stub(x => x.OrderByRandomOrder(playersInRepository)).Return(new[] { _player1, _player2 });
            _randomOrderer.Stub(x => x.OrderByRandomOrder(locations)).Return(new[] { _location3, _location2, _location1 });
        }

        [Test]
        public void Initializes_all_territories_with_owners()
        {
            var worldMap = _alternateGameSetup.Initialize();

            worldMap.GetTerritory(_location1).HasOwner().Should().BeTrue();
            worldMap.GetTerritory(_location2).HasOwner().Should().BeTrue();
            worldMap.GetTerritory(_location3).HasOwner().Should().BeTrue();
        }
    }
}