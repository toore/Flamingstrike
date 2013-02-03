using FluentAssertions;
using GuiWpf.Services;
using NUnit.Framework;

namespace RISK.Tests
{
    [TestFixture]
    public class ColorServiceTests
    {
        private ColorService _colorService;

        [SetUp]
        public void SetUp()
        {
            _colorService = new ColorService();
        }

        [Test]
        public void Has_North_America_continent_colors()
        {
            _colorService.NorthAmericaColors.Should().BeOfType<TerritoryColors>();
        }

        [Test]
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