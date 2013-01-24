using System;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class WorldMapTests
    {
        private WorldMap _worldMap;
        private ITerritoryLocationRepository _territoryLocationRepository;

        [SetUp]
        public void SetUp()
        {
            _territoryLocationRepository = MockRepository.GenerateStub<ITerritoryLocationRepository>();
            var scandinavia = new TerritoryLocation("scandinavia", new Continent());
            var congo = new TerritoryLocation("congo", new Continent());
            var egypt = new TerritoryLocation("egypt", new Continent());
            _territoryLocationRepository.Stub(x => x.Scandinavia).Return(scandinavia);
            _territoryLocationRepository.Stub(x => x.Congo).Return(congo);
            _territoryLocationRepository.Stub(x => x.Egypt).Return(egypt);
            _territoryLocationRepository.Stub(x => x.GetAll()).Return(new[] { scandinavia, congo, egypt });

            _worldMap = new WorldMap(_territoryLocationRepository);
        }

        [Test]
        public void GetTerritory_has_territory_for_scandinavia()
        {
            AssertGetTerritory(_territoryLocationRepository.Scandinavia);
        }

        [Test]
        public void GetTerritory_has_territory_for_congo()
        {
            AssertGetTerritory(_territoryLocationRepository.Congo);
        }

        [Test]
        public void GetTerritory_has_territory_for_egypt()
        {
            AssertGetTerritory(_territoryLocationRepository.Egypt);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetTerritory_throws_for_territory_Japan()
        {
            GetTerritory(_territoryLocationRepository.Japan);
        }

        private void AssertGetTerritory(ITerritoryLocation territoryLocation)
        {
            var territory = GetTerritory(territoryLocation);

            territory.Should().NotBeNull();
        }

        private ITerritory GetTerritory(ITerritoryLocation territoryLocation)
        {
            return _worldMap.GetTerritory(territoryLocation);
        }
    }
}