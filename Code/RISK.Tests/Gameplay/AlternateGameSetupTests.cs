using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

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
        private IRandomSorter _randomSorter;
        private IWorldMapFactory _worldMapFactory;
        private IWorldMap _worldMap;

        [SetUp]
        public void SetUp()
        {
            _playerRepository = Substitute.For<IPlayerRepository>();
            _locationProvider = Substitute.For<ILocationProvider>();
            _randomSorter = Substitute.For<IRandomSorter>();
            _worldMapFactory = Substitute.For<IWorldMapFactory>();

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            var playersInRepository = Enumerable.Empty<IPlayer>().ToList();
            _playerRepository.GetAll().Returns(playersInRepository);

            _location1 = Substitute.For<ILocation>();
            _location2 = Substitute.For<ILocation>();
            _location3 = Substitute.For<ILocation>();
            var locations = new[] { _location1, _location2, _location3 };
            _locationProvider.GetAll().Returns(locations);

            _worldMap = new WorldMap(_locationProvider);
            _worldMapFactory.Create().Returns(_worldMap);

            _alternateGameSetup = new AlternateGameSetup(_playerRepository, _locationProvider, _randomSorter, _worldMapFactory);

            _randomSorter.RandomSort(playersInRepository).Returns(new[] { _player1, _player2 });
            _randomSorter.RandomSort(locations).Returns(new[] { _location3, _location2, _location1 });
        }

        [Test]
        public void Creates_world_map()
        {
            var worldMap = _alternateGameSetup.Initialize();

            worldMap.Should().Be(_worldMap);
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