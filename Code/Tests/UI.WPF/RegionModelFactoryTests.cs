using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.RegionModels;
using FluentAssertions;
using Xunit;

namespace Tests.UI.WPF
{
    public class RegionModelFactoryTests
    {
        private readonly RegionModelFactory _sut;
        private const int ExpectedNumberOfModels = 42;

        public RegionModelFactoryTests()
        {
            var regions = new Regions();

            _sut = new RegionModelFactory(regions);
        }

        [Fact]
        public void Initializes_all_territory_models()
        {
            var territoryModels = _sut.Create();

            territoryModels.Should().HaveCount(ExpectedNumberOfModels);
        }

        [Fact]
        public void All_territory_models_have_unique_properties()
        {
            var territoryModels = _sut.Create()
                .ToList();

            territoryModels.Select(x => x.Region).Distinct().Should().HaveCount(ExpectedNumberOfModels);
            territoryModels.Select(x => x.Name).Distinct().Should().HaveCount(ExpectedNumberOfModels);
            territoryModels.Select(x => x.NamePosition).Distinct().Should().HaveCount(ExpectedNumberOfModels);
            territoryModels.Select(x => x.Path).Distinct().Should().HaveCount(ExpectedNumberOfModels);
        }
    }
}