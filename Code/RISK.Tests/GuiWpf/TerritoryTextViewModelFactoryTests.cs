using FluentAssertions;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class TerritoryTextViewModelFactoryTests
    {
        private TerritoryTextViewModelFactory _factory;
        private ITerritoryGuiFactory _territoryGuiFactory;

        [SetUp]
        public void SetUp()
        {
            _territoryGuiFactory = Substitute.For<ITerritoryGuiFactory>();

            _factory = new TerritoryTextViewModelFactory(_territoryGuiFactory);
        }

        [Test]
        public void Initialize()
        {
            var territory = Substitute.For<ITerritory>();
            var location = Substitute.For<ILocation>();
            territory.Location.Returns(location);
            territory.Armies = 99;

            var territoryTextViewModel = _factory.Create(territory);

            territoryTextViewModel.Location.Should().Be(location);
            territoryTextViewModel.Armies.Should().Be(99);
        }
    }
}