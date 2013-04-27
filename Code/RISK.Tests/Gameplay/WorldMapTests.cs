using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class WorldMapTests
    {
        private WorldMap _worldMap;
        private ILocationProvider _locationProvider;

        [SetUp]
        public void SetUp()
        {
            _locationProvider = Substitute.For<ILocationProvider>();
            var scandinavia = new Location("scandinavia", new Continent());
            var congo = new Location("congo", new Continent());
            var egypt = new Location("egypt", new Continent());
            _locationProvider.Scandinavia.Returns(scandinavia);
            _locationProvider.Congo.Returns(congo);
            _locationProvider.Egypt.Returns(egypt);
            _locationProvider.GetAll().Returns(new[] { scandinavia, congo, egypt });

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