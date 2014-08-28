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
            _colorService.NorthAmericaColors.Should().BeOfType<TerritoryColors>();
        }

        [Fact]
        public void Has_all_continents_colors()
        {
            _colorService.NorthAmericaColors.Should().BeOfType<TerritoryColors>();
            _colorService.SouthAmericaColors.Should().BeOfType<TerritoryColors>();
            _colorService.EuropeColors.Should().BeOfType<TerritoryColors>();
            _colorService.AfricaColors.Should().BeOfType<TerritoryColors>();
            _colorService.AsiaColors.Should().BeOfType<TerritoryColors>();
            _colorService.AustraliaColors.Should().BeOfType<TerritoryColors>();
        }
    }
}