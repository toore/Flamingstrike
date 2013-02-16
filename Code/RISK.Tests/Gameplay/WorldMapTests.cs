using System;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class WorldMapTests
    {
        private WorldMap _worldMap;
        private ILocationRepository _locationRepository;

        [SetUp]
        public void SetUp()
        {
            _locationRepository = MockRepository.GenerateStub<ILocationRepository>();
            var scandinavia = new Location("scandinavia", new Continent());
            var congo = new Location("congo", new Continent());
            var egypt = new Location("egypt", new Continent());
            _locationRepository.Stub(x => x.Scandinavia).Return(scandinavia);
            _locationRepository.Stub(x => x.Congo).Return(congo);
            _locationRepository.Stub(x => x.Egypt).Return(egypt);
            _locationRepository.Stub(x => x.GetAll()).Return(new[] { scandinavia, congo, egypt });

            _worldMap = new WorldMap(_locationRepository);
        }

        [Test]
        public void GetTerritory_has_territory_for_scandinavia()
        {
            AssertGetTerritory(_locationRepository.Scandinavia);
        }

        [Test]
        public void GetTerritory_has_territory_for_congo()
        {
            AssertGetTerritory(_locationRepository.Congo);
        }

        [Test]
        public void GetTerritory_has_territory_for_egypt()
        {
            AssertGetTerritory(_locationRepository.Egypt);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTerritory_throws_for_territory_Japan()
        {
            GetTerritory(_locationRepository.Japan);
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