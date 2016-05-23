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
            _colorService.NorthAmericaColors.Should().BeOfType<RegionColorSetting>();
        }

        [Fact]
        public void Has_all_continents_colors()
        {
            _colorService.NorthAmericaColors.Should().BeOfType<RegionColorSetting>();
            _colorService.SouthAmericaColors.Should().BeOfType<RegionColorSetting>();
            _colorService.EuropeColors.Should().BeOfType<RegionColorSetting>();
            _colorService.AfricaColors.Should().BeOfType<RegionColorSetting>();
            _colorService.AsiaColors.Should().BeOfType<RegionColorSetting>();
            _colorService.AustraliaColors.Should().BeOfType<RegionColorSetting>();
        }
    }
}