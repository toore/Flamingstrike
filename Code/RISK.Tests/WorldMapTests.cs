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
        public void GetArea_has_area_for_scandinavia()
        {
            AssertGetArea(_territoryLocationRepository.Scandinavia);
        }

        [Test]
        public void GetArea_has_area_for_congo()
        {
            AssertGetArea(_territoryLocationRepository.Congo);
        }

        [Test]
        public void GetArea_has_area_for_egypt()
        {
            AssertGetArea(_territoryLocationRepository.Egypt);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetArea_throws_for_area_Japan()
        {
            GetArea(_territoryLocationRepository.Japan);
        }

        private void AssertGetArea(ITerritoryLocation territoryLocation)
        {
            var area = GetArea(territoryLocation);

            area.Should().NotBeNull();
        }

        private ITerritory GetArea(ITerritoryLocation territoryLocation)
        {
            return _worldMap.GetArea(territoryLocation);
        }
    }
}