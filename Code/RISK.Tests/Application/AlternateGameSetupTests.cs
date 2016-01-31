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
        private readonly IRegion _territory1;
        private readonly IRegion _territory2;
        private readonly IRegion _territory3;
        private readonly ITerritoryResponder _territoryResponder;

        public AlternateGameSetupTests()
        {
            var worldMap = Substitute.For<IRegions>();
            var shuffler = Substitute.For<IShuffler>();
            var startingInfantryCalculator = Substitute.For<IStartingInfantryCalculator>();
            var players = new List<IPlayer> { null };
            _territoryResponder = Substitute.For<ITerritoryResponder>();

            _sut = new AlternateGameSetup(worldMap, players, startingInfantryCalculator, shuffler);
            _sut.TerritoryResponder = _territoryResponder;

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            shuffler.Shuffle(players).Returns(new[] { _player1, _player2 });

            var territories = new List<IRegion> { null };
            worldMap.GetAll().Returns(territories);
            _territory1 = Substitute.For<IRegion>();
            _territory2 = Substitute.For<IRegion>();
            _territory3 = Substitute.For<IRegion>();
            shuffler.Shuffle(territories).Returns(new[] { _territory1, _territory2, _territory3 });

            startingInfantryCalculator.Get(2).Returns(3);
        }

        [Fact]
        public void Shuffles_player_order()
        {
            RespondWithFirstEnabledTerritory();

            var actual = _sut.Initialize();

            actual.Players.Should().ContainInOrder(_player1, _player2);
        }

        [Fact]
        public void Shuffles_and_assigns_territories_to_players()
        {
            RespondWithFirstEnabledTerritory();

            var actual = _sut.Initialize();

            actual.Territories.ShouldAllBeEquivalentTo(new[]
            {
                new Territory(_territory1, _player1, 2),
                new Territory(_territory2, _player2, 3),
                new Territory(_territory3, _player1, 1)
            });
        }

        private void RespondWithFirstEnabledTerritory()
        {
            _territoryResponder.ProcessRequest(null)
                .ReturnsForAnyArgs(x => x.Arg<ITerritoryRequestParameter>().EnabledTerritories.First());
        }
    }
}