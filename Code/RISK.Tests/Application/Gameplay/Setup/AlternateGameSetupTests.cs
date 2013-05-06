using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;

namespace RISK.Tests.Application.Gameplay.Setup
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
        private IInitialArmyCountProvider _initialArmyCountProvider;
        private ILocationSelector _locationSelector;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private ITerritory _territory3;

        [SetUp]
        public void SetUp()
        {
            _playerRepository = Substitute.For<IPlayerRepository>();
            _locationProvider = Substitute.For<ILocationProvider>();
            _randomSorter = Substitute.For<IRandomSorter>();
            _worldMapFactory = Substitute.For<IWorldMapFactory>();
            _initialArmyCountProvider = Substitute.For<IInitialArmyCountProvider>();

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            var playerInRepository1 = Substitute.For<IPlayer>();
            var playerInRepository2 = Substitute.For<IPlayer>();
            var playersInRepository = new[] { playerInRepository1, playerInRepository2 };
            _playerRepository.GetAll().Returns(playersInRepository);

            _location1 = Substitute.For<ILocation>();
            _location2 = Substitute.For<ILocation>();
            _location3 = Substitute.For<ILocation>();
            var locations = new[] { _location1, _location2, _location3 };
            _locationProvider.GetAll().Returns(locations);

            _territory1 = Substitute.For<ITerritory>();
            _territory2 = Substitute.For<ITerritory>();
            _territory3 = Substitute.For<ITerritory>();

            _worldMap = Substitute.For<IWorldMap>();
            _worldMapFactory.Create().Returns(_worldMap);
            _worldMap.GetTerritory(_location1).Returns(_territory1);
            _worldMap.GetTerritory(_location2).Returns(_territory2);
            _worldMap.GetTerritory(_location3).Returns(_territory3);

            _initialArmyCountProvider.Get(2).Returns(3);

            _alternateGameSetup = new AlternateGameSetup(_playerRepository, _locationProvider, _randomSorter, _worldMapFactory, _initialArmyCountProvider);

            _randomSorter.Sort(Arg.Is<IEnumerable<IPlayer>>(x => x.SequenceEqual(playersInRepository))).Returns(new[] { _player1, _player2 });
            _randomSorter.Sort(locations).Returns(new[] { _location3, _location2, _location1 });

            _locationSelector = Substitute.For<ILocationSelector>();
        }

        [Test]
        public void Creates_world_map()
        {
            var worldMap = Initialize();

            worldMap.Should().Be(_worldMap);
        }

        [Test]
        public void Assign_players_to_territories()
        {
            var worldMap = Initialize();

            worldMap.GetTerritory(_location1).Occupant.Should().Be(_player1);
            worldMap.GetTerritory(_location2).Occupant.Should().Be(_player2);
            worldMap.GetTerritory(_location3).Occupant.Should().Be(_player1);
        }

        [Test]
        public void Place_armies_on_all_territories()
        {
            var player1Locations = new[] { _location3, _location1 };
            var player2Locations = new[] { _location2 };
            var player1Territories = new[] { _territory1, _territory3 };
            var player2Territories = new[] { _territory2 };
            _worldMap.GetTerritoriesOccupiedBy(_player1).Returns(player1Territories);
            _worldMap.GetTerritoriesOccupiedBy(_player2).Returns(player2Territories);
            
            //_locationSelector.Select(Arg.Is<List<ILocation>>(x => x.SequenceEqual(player1Locations))).Returns(_location3);
            //_locationSelector.Select(Arg.Is<List<ILocation>>(x => x.SequenceEqual(player2Locations))).Returns(_location2, _location2);
            _locationSelector.GetLocation(null).ReturnsForAnyArgs(_location3, _location2, _location2);

            var worldMap = Initialize();

            worldMap.GetTerritory(_location1).Armies.Should().Be(1);
            worldMap.GetTerritory(_location2).Armies.Should().Be(3);
            worldMap.GetTerritory(_location3).Armies.Should().Be(2);
        }

        private IWorldMap Initialize()
        {
            return _alternateGameSetup.Initialize(_locationSelector);
        }
    }
}