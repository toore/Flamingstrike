using FluentAssertions;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class WorldMapTests
    {
        private WorldMap _worldMap;
        private IAreaDefinitionProvider _areaDefinitionProvider;

        [SetUp]
        public void SetUp()
        {
            _areaDefinitionProvider = MockRepository.GenerateStub<IAreaDefinitionProvider>();
            var scandinavia = new AreaDefinition("scandinavia", new Continent());
            var congo = new AreaDefinition("congo", new Continent());
            var egypt = new AreaDefinition("egypt", new Continent());
            _areaDefinitionProvider.Stub(x => x.Scandinavia).Return(scandinavia);
            _areaDefinitionProvider.Stub(x => x.Congo).Return(congo);
            _areaDefinitionProvider.Stub(x => x.Egypt).Return(egypt);
            _areaDefinitionProvider.Stub(x => x.GetAll()).Return(new[] { scandinavia, congo, egypt });

            _worldMap = new WorldMap(_areaDefinitionProvider);
        }

        [Test]
        public void GetArea_has_area_for_scandinavia()
        {
            AssertGetArea(_areaDefinitionProvider.Scandinavia);
        }

        [Test]
        public void GetArea_has_area_for_congo()
        {
            AssertGetArea(_areaDefinitionProvider.Congo);
        }

        [Test]
        public void GetArea_has_area_for_egypt()
        {
            AssertGetArea(_areaDefinitionProvider.Egypt);
        }

        private void AssertGetArea(IAreaDefinition areaDefinition)
        {
            var area = _worldMap.GetArea(areaDefinition);

            area.Should().NotBeNull();
        }
    }
}