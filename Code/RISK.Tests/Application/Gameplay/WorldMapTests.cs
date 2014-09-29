using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class WorldMapTests
    {
        private readonly WorldMap _worldMap;
        private readonly Locations _locations;

        public WorldMapTests()
        {
            _locations = new Locations();
            _worldMap = new WorldMap(_locations);
        }

        [Fact]
        public void GetTerritory_has_territory_for_scandinavia()
        {
            AssertGetTerritory(_locations.Scandinavia);
        }

        [Fact]
        public void GetTerritory_has_territory_for_congo()
        {
            AssertGetTerritory(_locations.Congo);
        }

        [Fact]
        public void GetTerritory_has_territory_for_egypt()
        {
            AssertGetTerritory(_locations.Egypt);
        }

        [Fact]
        public void GetTerritory_throws_for_territory_Japan()
        {
            Action act = ()=>GetTerritory(new Location("unknown location", new Continent()));

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Get_players_occupying_territories_has_no_players()
        {
            _worldMap.GetAllPlayersOccupyingTerritories().Count().Should().Be(0);
        }

        [Fact]
        public void Two_players_is_occupying_territories()
        {
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();
            GetTerritory(_locations.Scandinavia).Occupant = player1;
            GetTerritory(_locations.Congo).Occupant = player2;

            var allPlayersOccupyingTerritories = _worldMap.GetAllPlayersOccupyingTerritories().ToList();

            allPlayersOccupyingTerritories.Count().Should().Be(2);
            allPlayersOccupyingTerritories.Should().BeEquivalentTo(player1, player2);
        }

        private void AssertGetTerritory(ILocation location)
        {
            var territory = GetTerritory(location);

            territory.Should().NotBeNull();
        }

        private ITerritory GetTerritory(ILocation location)
        {
            return _worldMap.GetTerritory(location);
        }
    }
}