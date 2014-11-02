using FluentAssertions;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application.Entities;
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
            territory.Armies = 99;

            var territoryTextViewModel = _factory.Create(territory);

            territoryTextViewModel.Territory.Should().Be(territory);
            territoryTextViewModel.Armies.Should().Be(99);
        }
    }
}