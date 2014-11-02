using System;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application;
using RISK.Application.Entities;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class TerritoryViewModelFactoryTests
    {
        private TerritoryViewModelFactory _territoryViewModelFactory;
        private Territories _territories;
        private Action<ITerritory> _action;
        private ITerritoryGuiFactory _territoryGuiFactory;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITerritory _siamTerritory;
        private ITerritoryGraphics _siamGraphics;

        public TerritoryViewModelFactoryTests()
        {
            _territories = new Territories();
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _territoryGuiFactory = Substitute.For<ITerritoryGuiFactory>();

            _territoryViewModelFactory = new TerritoryViewModelFactory(_territoryViewModelUpdater, _territoryGuiFactory);

            _action = Substitute.For<Action<ITerritory>>();

            _siamTerritory = _territories.Siam;
            
            _siamGraphics = Substitute.For<ITerritoryGraphics>();
            _siamGraphics.Path.Returns("siam path");
            _territoryGuiFactory.Create(_territories.Siam).Returns(_siamGraphics);
        }

        [Fact]
        public void Create_Siam_view_models_factory()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.Should().BeOfType<TerritoryLayoutViewModel>();
            viewModel.Path.Should().Be(_siamGraphics.Path);
            viewModel.IsEnabled.Should().BeTrue();

            _territoryViewModelUpdater.Received().UpdateColors(Arg.Any<ITerritoryLayoutViewModel>(), Arg.Is(_siamTerritory));
        }

        [Fact]
        public void OnClick_invokes_action()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.OnClick();

            _action.Received()(_territories.Siam);
        }

        private TerritoryLayoutViewModel CreateSiamTerritoryViewModel()
        {
            return _territoryViewModelFactory.Create(_siamTerritory, _action);
        }
    }
}