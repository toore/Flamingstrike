using System;
using System.Windows.Media;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.TerritoryViewModelFactories;
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
        private ITerritoryColorsFactory _territoryColorsFactory;
        private TerritoryColors _asiaColors;
        private byte _colorValue;
        private Action _action;
        private ITerritoryLayoutInformationFactory _territoryLayoutInformationFactory;

        [SetUp]
        public void SetUp()
        {
            _continentRepository = new ContinentRepository();
            _locationRepository = new LocationRepository(_continentRepository);
            _territoryColorsFactory = MockRepository.GenerateStub<ITerritoryColorsFactory>();
            _territoryLayoutInformationFactory = MockRepository.GenerateStub<ITerritoryLayoutInformationFactory>();
            _asiaColors = new TerritoryColors(GetNextColor(), GetNextColor(), GetNextColor(), GetNextColor());

            _territoryViewModelFactory = new TerritoryViewModelFactory(_territoryColorsFactory, _territoryLayoutInformationFactory);

            _action = () => { };
        }

        [Test]
        public void Create_Siam_view_models_factory()
        {
            var territory = MockRepository.GenerateStub<ITerritory>();
            territory.Stub(x => x.Location).Return(_locationRepository.Siam);
            _territoryColorsFactory.Stub(x => x.Create(territory)).Return(_asiaColors);
            var layoutInformationStub = MockRepository.GenerateStub<ITerritoryLayoutInformation>();
            _territoryLayoutInformationFactory.Stub(x => x.Create(_locationRepository.Siam)).Return(layoutInformationStub);

            var viewModel = _territoryViewModelFactory.Create(territory, _action);

            viewModel.Should().BeOfType<TerritoryViewModel>();
            viewModel.ClickCommand.Should().BeSameAs(_action);
            //TODO: Missing tests
            //viewModel.Path
            //viewModel.Path
            //viewModel.NormalStrokeColor
            //    viewModel.NormalStrokeColor
            //        viewModel.MouseOverStrokeColor
            //            viewModel.MouseOverFillColor
        }

        private Color GetNextColor()
        {
            _colorValue++;
            return Color.FromArgb(_colorValue, _colorValue, _colorValue, _colorValue);
        }
    }
}