using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GuiWpf.ViewModels.Settings;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.GameSetup;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Toore.Shuffling;
using Xunit;

namespace RISK.Tests.Application.Gameplay.Setup
{
    public class AlternateGameSetupTests
    {
        private readonly AlternateGameSetup _sut;
        private readonly IWorldMap _worldMap;
        private readonly IPlayerId _player1;
        private readonly IPlayerId _player2;
        private readonly ITerritoryRequestHandler _territoryRequestHandler;
        private readonly ITerritory _territory1;
        private readonly ITerritory _territory2;
        private readonly ITerritory _territory3;

        public AlternateGameSetupTests()
        {
            var playerRepository = Substitute.For<IPlayerRepository>();
            _worldMap = new WorldMap();
            var shuffle = Substitute.For<IShuffler>();
            //var worldMapFactory = Substitute.For<IWorldMapFactory>();
            var initialArmyForce = Substitute.For<IStartingInfantryCalculator>();

            var playerInRepository1 = Substitute.For<IPlayerId>();
            var playerInRepository2 = Substitute.For<IPlayerId>();
            playerRepository.GetAll().Returns(new[] { playerInRepository1, playerInRepository2 });

            var worldMapTerritory1 = Substitute.For<ITerritory>();
            var worldMapTerritory2 = Substitute.For<ITerritory>();
            var worldMapTerritory3 = Substitute.For<ITerritory>();

            _worldMap = Substitute.For<IWorldMap>();
            //worldMapFactory.Create().Returns(_worldMap);
            _worldMap.GetTerritories().Returns(new[] { worldMapTerritory1, worldMapTerritory2, worldMapTerritory3 });

            initialArmyForce.Get(2).Returns(3);

            _sut = new AlternateGameSetup(null,null,null,null);
                //new AlternateGameSetup(playerRepository, worldMapFactory, shuffle, initialArmyForce);

            _player1 = Substitute.For<IPlayerId>();
            _player2 = Substitute.For<IPlayerId>();
            shuffle.Shuffle(Arg.Is<IEnumerable<IPlayerId>>(x => x.SequenceEqual(new[] { playerInRepository1, playerInRepository2 }))).Returns(new[] { _player1, _player2 });

            _territory1 = Substitute.For<ITerritory>();
            _territory2 = Substitute.For<ITerritory>();
            _territory3 = Substitute.For<ITerritory>();
            shuffle.Shuffle(Arg.Is<IEnumerable<ITerritory>>(x => x.SequenceEqual(new[] { worldMapTerritory1, worldMapTerritory2, worldMapTerritory3 }))).Returns(new[] { _territory1, _territory2, _territory3 });

            _territoryRequestHandler = Substitute.For<ITerritoryRequestHandler>();
        }

        [Fact]
        public void Initializes_territories()
        {
            _territoryRequestHandler.ProcessRequest(null).ReturnsForAnyArgs(Substitute.For<ITerritory>());
            var actual = _sut.Initialize(_territoryRequestHandler);

            actual.Should().Be(_worldMap);
        }

        [Fact]
        public void Assign_players_to_territories()
        {
            _sut.Initialize(_territoryRequestHandler);

            //_territory1.Occupant.Should().Be(_player1);
            //_territory2.Occupant.Should().Be(_player2);
            //_territory3.Occupant.Should().Be(_player1);
            throw new NotImplementedException();
        }

        [Fact]
        public void Place_armies_on_all_territories()
        {
            var player1Territories = new[] { _territory1, _territory3 };
            var player2Territories = new[] { _territory2 };
            //_worldMap.GetTerritoriesOccupiedByPlayer(_player1).Returns(player1Territories);
            //_worldMap.GetTerritoriesOccupiedByPlayer(_player2).Returns(player2Territories);

            _territoryRequestHandler.ProcessRequest(null).ReturnsForAnyArgs(_territory2, _territory3, _territory2);

            _sut.Initialize(_territoryRequestHandler);

            //_territory1.Armies.Should().Be(1);
            //_territory2.Armies.Should().Be(3);
            //_territory3.Armies.Should().Be(2);
            throw new NotImplementedException();
        }
    }
}