using FluentAssertions;
using GuiWpf.Services;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    
    public class ColorServiceTests
    {
        private readonly ColorService _colorService;

        public ColorServiceTests()
        {
            _colorService = new ColorService();
        }

        [Fact]
        public void Has_North_America_continent_colors()
        {
            _colorService.NorthAmericaColors.Should().BeOfType<RegionColorSettings>();
        }

        [Fact]
        public void Has_all_continents_colors()
        {
            _colorService.NorthAmericaColors.Should().BeOfType<RegionColorSettings>();
            _colorService.SouthAmericaColors.Should().BeOfType<RegionColorSettings>();
            _colorService.EuropeColors.Should().BeOfType<RegionColorSettings>();
            _colorService.AfricaColors.Should().BeOfType<RegionColorSettings>();
            _colorService.AsiaColors.Should().BeOfType<RegionColorSettings>();
            _colorService.AustraliaColors.Should().BeOfType<RegionColorSettings>();
        }
    }
}