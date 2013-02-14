using System;
using FluentAssertions;
using GuiWpf.GuiDefinitions;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameboard.WorldMap;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class TerritoryViewModelFactoryTests
    {
        private TerritoryViewModelFactory _territoryViewModelFactory;
        private ILocationRepository _locationRepository;
        private IContinentRepository _continentRepository;
        private Action<ILocation> _action;
        private ITerritoryGuiDefinitionFactory _territoryGuiDefinitionFactory;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITerritory _siamTerritory;
        private ITerritoryGuiDefinitions _siamGuiDefinitions;

        [SetUp]
        public void SetUp()
        {
            _continentRepository = new ContinentRepository();
            _locationRepository = new LocationRepository(_continentRepository);
            _territoryViewModelUpdater = MockRepository.GenerateStub<ITerritoryViewModelUpdater>();
            _territoryGuiDefinitionFactory = MockRepository.GenerateStub<ITerritoryGuiDefinitionFactory>();

            _territoryViewModelFactory = new TerritoryViewModelFactory(_territoryViewModelUpdater, _territoryGuiDefinitionFactory);

            _action = MockRepository.GenerateMock<Action<ILocation>>();

            _siamTerritory = MockRepository.GenerateStub<ITerritory>();
            _siamTerritory.Stub(x => x.Location).Return(_locationRepository.Siam);

            _siamGuiDefinitions = MockRepository.GenerateStub<ITerritoryGuiDefinitions>();
            _siamGuiDefinitions.Stub(x => x.Path).Return("siam path");
            _territoryGuiDefinitionFactory.Stub(x => x.Create(_locationRepository.Siam)).Return(_siamGuiDefinitions);
        }

        [Test]
        public void Create_Siam_view_models_factory()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.Should().BeOfType<TerritoryViewModel>();
            viewModel.Path.Should().Be(_siamGuiDefinitions.Path);
            viewModel.IsEnabled.Should().BeTrue();
            _territoryViewModelUpdater.AssertWasCalled(x => x.UpdateColor(Arg<ITerritoryViewModel>.Is.Anything, Arg<ITerritory>.Is.Equal(_siamTerritory)));
        }

        [Test]
        public void OnClick_invokes_action()
        {
            var viewModel = CreateSiamTerritoryViewModel();

            viewModel.OnClick();

            _action.AssertWasCalled(x => x(_locationRepository.Siam));
        }

        private TerritoryViewModel CreateSiamTerritoryViewModel()
        {
            return _territoryViewModelFactory.Create(_siamTerritory, _action);
        }
    }
}