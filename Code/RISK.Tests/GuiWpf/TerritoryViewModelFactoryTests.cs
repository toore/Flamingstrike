using System;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain;
using RISK.Domain.Entities;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class TerritoryViewModelFactoryTests
    {
        private TerritoryViewModelFactory _territoryViewModelFactory;
        private Locations _locations;
        private Action<ILocation> _action;
        private ITerritoryGuiFactory _territoryGuiFactory;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITerritory _siamTerritory;
        private ITerritoryGraphics _siamGraphics;

        [SetUp]
        public void SetUp()
        {
            var continents = new Continents();
            _locations = new Locations(continents);
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _territoryGuiFactory = Substitute.For<ITerritoryGuiFactory>();

            _territoryViewModelFactory = new TerritoryViewModelFactory(_territoryViewModelUpdater, _territoryGuiFactory);

            _action = Substitute.For<Action<ILocation>>();

            _siamTerritory = Substitute.For<ITerritory>();
            _siamTerritory.Location.Returns(_locations.Siam);

            _siamGraphics = Substitute.For<ITerritoryGraphics>();
            _siamGraphics.Path.Returns("siam path");
            _territoryGuiFactory.Create(_locations.Siam).Returns(_siamGraphics);
        }

        [Test]
        public void Create_Siam_view_models_factory()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.Should().BeOfType<TerritoryLayoutViewModel>();
            viewModel.Path.Should().Be(_siamGraphics.Path);
            viewModel.IsEnabled.Should().BeTrue();

            _territoryViewModelUpdater.Received().UpdateColors(Arg.Any<ITerritoryLayoutViewModel>(), Arg.Is(_siamTerritory));
        }

        [Test]
        public void OnClick_invokes_action()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.OnClick();

            _action.Received()(_locations.Siam);
        }

        private TerritoryLayoutViewModel CreateSiamTerritoryViewModel()
        {
            return _territoryViewModelFactory.Create(_siamTerritory, _action);
        }
    }
}