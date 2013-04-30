using System;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class TerritoryViewModelFactoryTests
    {
        private TerritoryViewModelFactory _territoryViewModelFactory;
        private ILocationProvider _locationProvider;
        private IContinentProvider _continentProvider;
        private Action<ILocation> _action;
        private ITerritoryGuiFactory _territoryGuiFactory;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITerritory _siamTerritory;
        private ITerritoryGui _siamGui;

        [SetUp]
        public void SetUp()
        {
            _continentProvider = new ContinentProvider();
            _locationProvider = new LocationProvider(_continentProvider);
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _territoryGuiFactory = Substitute.For<ITerritoryGuiFactory>();

            _territoryViewModelFactory = new TerritoryViewModelFactory(_territoryViewModelUpdater, _territoryGuiFactory);

            _action = Substitute.For<Action<ILocation>>();

            _siamTerritory = Substitute.For<ITerritory>();
            _siamTerritory.Location.Returns(_locationProvider.Siam);

            _siamGui = Substitute.For<ITerritoryGui>();
            _siamGui.Path.Returns("siam path");
            _territoryGuiFactory.Create(_locationProvider.Siam).Returns(_siamGui);
        }

        [Test]
        public void Create_Siam_view_models_factory()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.Should().BeOfType<TerritoryLayoutViewModel>();
            viewModel.Path.Should().Be(_siamGui.Path);
            viewModel.IsEnabled.Should().BeTrue();

            _territoryViewModelUpdater.Received().UpdateColors(Arg.Any<ITerritoryLayoutViewModel>(), Arg.Is(_siamTerritory));
        }

        [Test]
        public void OnClick_invokes_action()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.OnClick();

            _action.Received()(_locationProvider.Siam);
        }

        private TerritoryLayoutViewModel CreateSiamTerritoryViewModel()
        {
            return _territoryViewModelFactory.Create(_siamTerritory, _action);
        }
    }
}