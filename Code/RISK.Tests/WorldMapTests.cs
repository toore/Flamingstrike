using System;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class WorldMapTests
    {
        private WorldMap _worldMap;
        private IAreaDefinitionRepository _areaDefinitionRepository;

        [SetUp]
        public void SetUp()
        {
            _areaDefinitionRepository = MockRepository.GenerateStub<IAreaDefinitionRepository>();
            var scandinavia = new AreaDefinition("scandinavia", new Continent());
            var congo = new AreaDefinition("congo", new Continent());
            var egypt = new AreaDefinition("egypt", new Continent());
            _areaDefinitionRepository.Stub(x => x.Scandinavia).Return(scandinavia);
            _areaDefinitionRepository.Stub(x => x.Congo).Return(congo);
            _areaDefinitionRepository.Stub(x => x.Egypt).Return(egypt);
            _areaDefinitionRepository.Stub(x => x.GetAll()).Return(new[] { scandinavia, congo, egypt });

            _worldMap = new WorldMap(_areaDefinitionRepository);
        }

        [Test]
        public void GetArea_has_area_for_scandinavia()
        {
            AssertGetArea(_areaDefinitionRepository.Scandinavia);
        }

        [Test]
        public void GetArea_has_area_for_congo()
        {
            AssertGetArea(_areaDefinitionRepository.Congo);
        }

        [Test]
        public void GetArea_has_area_for_egypt()
        {
            AssertGetArea(_areaDefinitionRepository.Egypt);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetArea_throws_for_area_Japan()
        {
            GetArea(_areaDefinitionRepository.Japan);
        }

        private void AssertGetArea(IAreaDefinition areaDefinition)
        {
            var area = GetArea(areaDefinition);

            area.Should().NotBeNull();
        }

        private IArea GetArea(IAreaDefinition areaDefinition)
        {
            return _worldMap.GetArea(areaDefinition);
        }
    }
}