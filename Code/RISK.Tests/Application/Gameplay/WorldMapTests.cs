using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class WorldMapTests
    {
        private WorldMap _worldMap;
        private ILocationProvider _locationProvider;
        private Location _scandinavia;
        private Location _congo;
        private Location _egypt;

        [SetUp]
        public void SetUp()
        {
            _locationProvider = Substitute.For<ILocationProvider>();
            _scandinavia = new Location("scandinavia", new Continent());
            _congo = new Location("congo", new Continent());
            _egypt = new Location("egypt", new Continent());
            _locationProvider.Scandinavia.Returns(_scandinavia);
            _locationProvider.Congo.Returns(_congo);
            _locationProvider.Egypt.Returns(_egypt);
            _locationProvider.GetAll().Returns(new[] { _scandinavia, _congo, _egypt });

            _worldMap = new WorldMap(_locationProvider);
        }

        [Test]
        public void GetTerritory_has_territory_for_scandinavia()
        {
            AssertGetTerritory(_locationProvider.Scandinavia);
        }

        [Test]
        public void GetTerritory_has_territory_for_congo()
        {
            AssertGetTerritory(_locationProvider.Congo);
        }

        [Test]
        public void GetTerritory_has_territory_for_egypt()
        {
            AssertGetTerritory(_locationProvider.Egypt);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTerritory_throws_for_territory_Japan()
        {
            GetTerritory(_locationProvider.Japan);
        }

        [Test]
        public void Get_players_occupying_territories_has_no_players()
        {
            _worldMap.GetAllPlayersOccupyingTerritories().Count().Should().Be(0);
        }

        [Test]
        public void Two_players_is_occupying_territories()
        {
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();
            GetTerritory(_scandinavia).Occupant = player1;
            GetTerritory(_congo).Occupant = player2;

            var allPlayersOccupyingTerritories = _worldMap.GetAllPlayersOccupyingTerritories();
            
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