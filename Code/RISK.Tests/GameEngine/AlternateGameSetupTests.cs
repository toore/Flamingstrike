using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Setup;
using RISK.Tests.Builders;
using Toore.Shuffling;
using Xunit;
using IPlayer = RISK.GameEngine.Play.IPlayer;

namespace RISK.Tests.GameEngine
{
    public class AlternateGameSetupTests
    {
        private readonly AlternateGameSetup _sut;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private readonly IRegion _region1;
        private readonly IRegion _region2;
        private readonly IRegion _region3;
        private readonly ITerritoryResponder _territoryResponder;

        public AlternateGameSetupTests()
        {
            var worldMap = Substitute.For<IRegions>();
            var shuffler = Substitute.For<IShuffle>();
            var startingInfantryCalculator = Substitute.For<IStartingInfantryCalculator>();
            var players = new List<IPlayer> { null };
            _territoryResponder = Substitute.For<ITerritoryResponder>();

            _sut = new AlternateGameSetup(worldMap, players, startingInfantryCalculator, shuffler);
            _sut.TerritoryResponder = _territoryResponder;

            _player1 = Make.Player.Build();
            _player2 = Make.Player.Build();
            shuffler.Shuffle(players).Returns(new[] { _player1, _player2 });

            var regions = new List<IRegion> { null };
            worldMap.GetAll().Returns(regions);
            _region1 = Substitute.For<IRegion>();
            _region2 = Substitute.For<IRegion>();
            _region3 = Substitute.For<IRegion>();
            shuffler.Shuffle(regions).Returns(new[] { _region1, _region2, _region3 });

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
                new Territory(_region1, _player1, 2),
                new Territory(_region2, _player2, 3),
                new Territory(_region3, _player1, 1)
            });
        }

        private void RespondWithFirstEnabledTerritory()
        {
            _territoryResponder.ProcessRequest(null)
                .ReturnsForAnyArgs(x => x.Arg<ITerritoryRequestParameter>().EnabledTerritories.First());
        }
    }
}