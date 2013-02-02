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
            _colorService.NorthAmericaColors.Should().BeOfType<ContinentColors>();
        }

        [Test]
        public void Has_all_continents_colors()
        {
            _colorService.NorthAmericaColors.Should().BeOfType<ContinentColors>();
            _colorService.SouthAmericaColors.Should().BeOfType<ContinentColors>();
            _colorService.EuropeColors.Should().BeOfType<ContinentColors>();
            _colorService.AfricaColors.Should().BeOfType<ContinentColors>();
            _colorService.AsiaColors.Should().BeOfType<ContinentColors>();
            _colorService.AustraliaColors.Should().BeOfType<ContinentColors>();
        }
    }
}