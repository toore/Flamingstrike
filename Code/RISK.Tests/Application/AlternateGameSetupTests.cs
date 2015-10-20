using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Setup;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public class AlternateGameSetupTests
    {
        private readonly AlternateGameSetup _sut;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private readonly ITerritory _territory1;
        private readonly ITerritory _territory2;
        private readonly ITerritory _territory3;
        private readonly ITerritoryRequestHandler _territoryRequestHandler;

        public AlternateGameSetupTests()
        {
            var worldMap = Substitute.For<IWorldMap>();
            var shuffler = Substitute.For<IShuffler>();
            var startingInfantryCalculator = Substitute.For<IStartingInfantryCalculator>();
            var players = new List<IPlayer> { null };

            _sut = new AlternateGameSetup(worldMap, players, startingInfantryCalculator, shuffler);

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            shuffler.Shuffle(players).Returns(new[] { _player1, _player2 });

            var territories = new List<ITerritory> { null };
            worldMap.GetAll().Returns(territories);
            _territory1 = Substitute.For<ITerritory>();
            _territory2 = Substitute.For<ITerritory>();
            _territory3 = Substitute.For<ITerritory>();
            shuffler.Shuffle(territories).Returns(new[] { _territory1, _territory2, _territory3 });

            startingInfantryCalculator.Get(2).Returns(3);

            _territoryRequestHandler = Substitute.For<ITerritoryRequestHandler>();
        }

        [Fact]
        public void Shuffles_player_order()
        {
            ForAnyTerritoryRequestReturnFirstValidTerritory();

            var actual = _sut.Initialize(_territoryRequestHandler);

            actual.Players.Should().ContainInOrder(_player1, _player2);
        }

        [Fact]
        public void Shuffles_and_assigns_territories_to_players()
        {
            ForAnyTerritoryRequestReturnFirstValidTerritory();

            var actual = _sut.Initialize(_territoryRequestHandler);

            actual.GameboardSetupTerritories.ShouldAllBeEquivalentToInRisk(new[]
            {
                new GameboardSetupTerritory(_territory1, _player1, 2),
                new GameboardSetupTerritory(_territory2, _player2, 3),
                new GameboardSetupTerritory(_territory3, _player1, 1)
            });
        }

        private void ForAnyTerritoryRequestReturnFirstValidTerritory()
        {
            _territoryRequestHandler.ProcessRequest(null)
                .ReturnsForAnyArgs(x => x.Arg<ITerritoryRequestParameter>().EnabledTerritories.First());
        }
    }
}