using System;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
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
        private WorldMap _worldMap;
        private Action<ITerritory> _action;
        private ITerritoryGuiFactory _territoryGuiFactory;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITerritory _siamTerritory;
        private ITerritoryModel _siamModel;

        public TerritoryViewModelFactoryTests()
        {
            _worldMap = new WorldMap();
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _territoryGuiFactory = Substitute.For<ITerritoryGuiFactory>();

            _territoryViewModelFactory = new TerritoryViewModelFactory(_territoryViewModelUpdater, _territoryGuiFactory);

            _action = Substitute.For<Action<ITerritory>>();

            _siamTerritory = _worldMap.Siam;
            
            _siamModel = Substitute.For<ITerritoryModel>();
            _siamModel.Path.Returns("siam path");
            _territoryGuiFactory.Create(_worldMap.Siam).Returns(_siamModel);
        }

        [Fact]
        public void Create_Siam_view_models_factory()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.Should().BeOfType<TerritoryLayoutViewModel>();
            viewModel.Path.Should().Be(_siamModel.Path);
            viewModel.IsEnabled.Should().BeTrue();

            _territoryViewModelUpdater.Received().UpdateColors(Arg.Any<ITerritoryLayoutViewModel>(), Arg.Is(_siamTerritory));
        }

        [Fact]
        public void OnClick_invokes_action()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.OnClick();

            _action.Received()(_worldMap.Siam);
        }

        private TerritoryLayoutViewModel CreateSiamTerritoryViewModel()
        {
            return _territoryViewModelFactory.Create(_siamTerritory, _action);
        }
    }
}