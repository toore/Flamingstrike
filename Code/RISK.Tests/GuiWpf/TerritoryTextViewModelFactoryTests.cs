using FluentAssertions;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Domain.Entities;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class TerritoryTextViewModelFactoryTests
    {
        private TerritoryTextViewModelFactory _factory;
        private ITerritoryGuiFactory _territoryGuiFactory;

        public TerritoryTextViewModelFactoryTests()
        {
            _territoryGuiFactory = Substitute.For<ITerritoryGuiFactory>();

            _factory = new TerritoryTextViewModelFactory(_territoryGuiFactory);
        }

        [Fact]
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