using FluentAssertions;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application.Entities;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class TerritoryTextViewModelFactoryTests
    {
        private TerritoryTextViewModelFactory _factory;
        private IWorldMapModelFactory _worldMapModelFactory;

        public TerritoryTextViewModelFactoryTests()
        {
            _worldMapModelFactory = Substitute.For<IWorldMapModelFactory>();

            _factory = new TerritoryTextViewModelFactory(_worldMapModelFactory);
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