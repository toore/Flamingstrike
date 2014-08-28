using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Gameplay.Setup
{
    public class AlternateGameSetupTests
    {
        private AlternateGameSetup _alternateGameSetup;
        private IPlayers _players;
        private Locations _locations;
        private ILocation _location1;
        private ILocation _location2;
        private ILocation _location3;
        private IPlayer _player1;
        private IPlayer _player2;
        private IRandomSorter _randomSorter;
        private IWorldMapFactory _worldMapFactory;
        private IWorldMap _worldMap;
        private IInitialArmyCount _initialArmyCount;
        private IGameInitializerLocationSelector _gameInitializerLocationSelector;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private ITerritory _territory3;

        public AlternateGameSetupTests()
        {
            _players = Substitute.For<IPlayers>();
            _locations = new Locations(new Continents());
            _randomSorter = Substitute.For<IRandomSorter>();
            _worldMapFactory = Substitute.For<IWorldMapFactory>();
            _initialArmyCount = Substitute.For<IInitialArmyCount>();

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            var playerInRepository1 = Substitute.For<IPlayer>();
            var playerInRepository2 = Substitute.For<IPlayer>();
            var playersInRepository = new[] { playerInRepository1, playerInRepository2 };
            _players.GetAll().Returns(playersInRepository);

            _location1 = Substitute.For<ILocation>();
            _location2 = Substitute.For<ILocation>();
            _location3 = Substitute.For<ILocation>();

            _territory1 = Substitute.For<ITerritory>();
            _territory2 = Substitute.For<ITerritory>();
            _territory3 = Substitute.For<ITerritory>();

            _worldMap = Substitute.For<IWorldMap>();
            _worldMapFactory.Create().Returns(_worldMap);
            _worldMap.GetTerritory(_location1).Returns(_territory1);
            _worldMap.GetTerritory(_location2).Returns(_territory2);
            _worldMap.GetTerritory(_location3).Returns(_territory3);

            _initialArmyCount.Get(2).Returns(3);

            _alternateGameSetup = new AlternateGameSetup(_players, _locations, _randomSorter, _worldMapFactory, _initialArmyCount);

            _randomSorter.Sort(Arg.Is<IEnumerable<IPlayer>>(x => x.SequenceEqual(playersInRepository))).Returns(new[] { _player1, _player2 });
            _randomSorter.Sort(Arg.Is<IEnumerable<ILocation>>(x => x.SequenceEqual(_locations.GetAll()))).Returns(new[] { _location3, _location2, _location1 });

            _gameInitializerLocationSelector = Substitute.For<IGameInitializerLocationSelector>();
        }

        [Fact]
        public void Creates_world_map()
        {
            var worldMap = Initialize();

            worldMap.Should().Be(_worldMap);
        }

        [Fact]
        public void Assign_players_to_territories()
        {
            var worldMap = Initialize();

            worldMap.GetTerritory(_location1).Occupant.Should().Be(_player1);
            worldMap.GetTerritory(_location2).Occupant.Should().Be(_player2);
            worldMap.GetTerritory(_location3).Occupant.Should().Be(_player1);
        }

        [Fact]
        public void Place_armies_on_all_territories()
        {
            var player1Territories = new[] { _territory1, _territory3 };
            var player2Territories = new[] { _territory2 };
            _worldMap.GetTerritoriesOccupiedBy(_player1).Returns(player1Territories);
            _worldMap.GetTerritoriesOccupiedBy(_player2).Returns(player2Territories);

            _gameInitializerLocationSelector.SelectLocation(null).ReturnsForAnyArgs(_location3, _location2, _location2);

            var worldMap = Initialize();

            worldMap.GetTerritory(_location1).Armies.Should().Be(1);
            worldMap.GetTerritory(_location2).Armies.Should().Be(3);
            worldMap.GetTerritory(_location3).Armies.Should().Be(2);
        }

        private IWorldMap Initialize()
        {
            return _alternateGameSetup.Initialize(_gameInitializerLocationSelector);
        }
    }
}