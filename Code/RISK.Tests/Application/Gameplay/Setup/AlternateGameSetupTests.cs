using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;
using Toore.Shuffling;
using Xunit;

namespace RISK.Tests.Application.Gameplay.Setup
{
    public class AlternateGameSetupTests
    {
        private readonly AlternateGameSetup _sut;
        private readonly IWorldMap _worldMap;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private readonly ITerritorySelector _territorySelector;
        private readonly ITerritory _territory1;
        private readonly ITerritory _territory2;
        private readonly ITerritory _territory3;

        public AlternateGameSetupTests()
        {
            var players = Substitute.For<IPlayerRepository>();
            _worldMap = new WorldMap();
            var shuffleAlgorithm = Substitute.For<IShuffle>();
            var worldMapFactory = Substitute.For<IWorldMapFactory>();
            var initialArmyAssignmentCalculator = Substitute.For<IInitialArmyAssignmentCalculator>();

            var playerInRepository1 = Substitute.For<IPlayer>();
            var playerInRepository2 = Substitute.For<IPlayer>();
            players.GetAll().Returns(new[] { playerInRepository1, playerInRepository2 });

            var worldMapTerritory1 = Substitute.For<ITerritory>();
            var worldMapTerritory2 = Substitute.For<ITerritory>();
            var worldMapTerritory3 = Substitute.For<ITerritory>();

            _worldMap = Substitute.For<IWorldMap>();
            worldMapFactory.Create().Returns(_worldMap);
            _worldMap.GetTerritories().Returns(new[] { worldMapTerritory1, worldMapTerritory2, worldMapTerritory3 });

            initialArmyAssignmentCalculator.Get(2).Returns(3);

            _sut = new AlternateGameSetup(players, worldMapFactory, shuffleAlgorithm, initialArmyAssignmentCalculator);

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            shuffleAlgorithm.Shuffle(Arg.Is<IEnumerable<IPlayer>>(x => x.SequenceEqual(new[] { playerInRepository1, playerInRepository2 }))).Returns(new[] { _player1, _player2 });

            _territory1 = Substitute.For<ITerritory>();
            _territory2 = Substitute.For<ITerritory>();
            _territory3 = Substitute.For<ITerritory>();
            shuffleAlgorithm.Shuffle(Arg.Is<IEnumerable<ITerritory>>(x => x.SequenceEqual(new[] { worldMapTerritory1, worldMapTerritory2, worldMapTerritory3 }))).Returns(new[] { _territory1, _territory2, _territory3 });

            _territorySelector = Substitute.For<ITerritorySelector>();
        }

        [Fact]
        public void Initializes_territories()
        {
            _territorySelector.SelectTerritory(null).ReturnsForAnyArgs(Substitute.For<ITerritory>());
            var actual = _sut.InitializeWorldMap(_territorySelector);

            actual.Should().Be(_worldMap);
        }

        [Fact]
        public void Assign_players_to_territories()
        {
            _sut.InitializeWorldMap(_territorySelector);

            _territory1.Occupant.Should().Be(_player1);
            _territory2.Occupant.Should().Be(_player2);
            _territory3.Occupant.Should().Be(_player1);
        }

        [Fact]
        public void Place_armies_on_all_territories()
        {
            var player1Territories = new[] { _territory1, _territory3 };
            var player2Territories = new[] { _territory2 };
            _worldMap.GetTerritoriesOccupiedByPlayer(_player1).Returns(player1Territories);
            _worldMap.GetTerritoriesOccupiedByPlayer(_player2).Returns(player2Territories);

            _territorySelector.SelectTerritory(null).ReturnsForAnyArgs(_territory2, _territory3, _territory2);

            _sut.InitializeWorldMap(_territorySelector);

            _territory1.Armies.Should().Be(1);
            _territory2.Armies.Should().Be(3);
            _territory3.Armies.Should().Be(2);
        }
    }
}