using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Gameplay.Setup
{
    public class AlternateGameSetupTests
    {
        private AlternateGameSetup _sut;
        private IPlayers _players;
        private Territories _territories;
        private ITerritory _location1;
        private ITerritory _location2;
        private ITerritory _location3;
        private IPlayer _player1;
        private IPlayer _player2;
        private IRandomSorter _randomSorter;
        private ITerritoriesFactory _territoriesFactory;
        private IInitialArmyCount _initialArmyCount;
        private IGameInitializerLocationSelector _gameInitializerLocationSelector;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private ITerritory _territory3;

        public AlternateGameSetupTests()
        {
            _players = Substitute.For<IPlayers>();
            _territories = new Territories();
            _randomSorter = Substitute.For<IRandomSorter>();
            _territoriesFactory = Substitute.For<ITerritoriesFactory>();
            _initialArmyCount = Substitute.For<IInitialArmyCount>();

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            var playerInRepository1 = Substitute.For<IPlayer>();
            var playerInRepository2 = Substitute.For<IPlayer>();
            var playersInRepository = new[] { playerInRepository1, playerInRepository2 };
            _players.GetAll().Returns(playersInRepository);

            _territory1 = Substitute.For<ITerritory>();
            _territory2 = Substitute.For<ITerritory>();
            _territory3 = Substitute.For<ITerritory>();

            _territories = new Territories();
            _territoriesFactory.Create().Returns(_territories);

            _initialArmyCount.Get(2).Returns(3);

            _sut = new AlternateGameSetup(_players, _territories, _randomSorter, _territoriesFactory, _initialArmyCount);

            _randomSorter.Sort(Arg.Is<IEnumerable<IPlayer>>(x => x.SequenceEqual(playersInRepository))).Returns(new[] { _player1, _player2 });
            _randomSorter.Sort(Arg.Is<IEnumerable<ITerritory>>(x => x.SequenceEqual(_territories.GetAll()))).Returns(new[] { _location3, _location2, _location1 });

            _gameInitializerLocationSelector = Substitute.For<IGameInitializerLocationSelector>();
        }

        [Fact]
        public void Initializes_territories()
        {
            var territories = _sut.Initialize(_gameInitializerLocationSelector);

            territories.Should().Be(_territories);
        }

        [Fact]
        public void Assign_players_to_territories()
        {
            _sut.Initialize(_gameInitializerLocationSelector);

            _territory1.Occupant.Should().Be(_player1);
            _territory2.Occupant.Should().Be(_player2);
            _territory3.Occupant.Should().Be(_player1);
        }

        [Fact]
        public void Place_armies_on_all_territories()
        {
            var player1Territories = new[] { _territory1, _territory3 };
            var player2Territories = new[] { _territory2 };
            _territories.GetTerritoriesOccupiedByPlayer(_player1).Returns(player1Territories);
            _territories.GetTerritoriesOccupiedByPlayer(_player2).Returns(player2Territories);

            _gameInitializerLocationSelector.SelectLocation(null).ReturnsForAnyArgs(_location3, _location2, _location2);

            var worldMap = _sut.Initialize(_gameInitializerLocationSelector);

            _territory1.Armies.Should().Be(1);
            _territory2.Armies.Should().Be(3);
            _territory3.Armies.Should().Be(2);
        }
    }
}