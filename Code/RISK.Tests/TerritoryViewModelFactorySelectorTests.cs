using System.Windows.Media;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.Views.WorldMap.TerritoryViewModelFactories;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class TerritoryViewModelFactorySelectorTests
    {
        private TerritoryViewModelsFactorySelector _territoryViewModelsFactorySelector;
        private ILocationRepository _locationRepository;
        private IContinentRepository _continentRepository;
        private IColorService _colorService;
        private TerritoryColors _asiaColors;
        private byte _colorValue;

        [SetUp]
        public void SetUp()
        {
            _continentRepository = new ContinentRepository();
            _locationRepository = new LocationRepository(_continentRepository);
            _colorService = MockRepository.GenerateStub<IColorService>();

            _asiaColors = new TerritoryColors(GetNextColor(), GetNextColor(), GetNextColor(), GetNextColor());
            _colorService.Stub(x => x.AsiaColors).Return(_asiaColors);

            _territoryViewModelsFactorySelector = new TerritoryViewModelsFactorySelector(_locationRepository, _colorService);
        }

        [Test]
        public void Create_Siam_view_models_factory()
        {
            var territory = MockRepository.GenerateStub<ITerritory>();
            territory.Stub(x => x.Location).Return(_locationRepository.Siam);

            var territoryViewModelsFactory = _territoryViewModelsFactorySelector.Select(territory);

            territoryViewModelsFactory.Should().BeOfType<SiamViewModelsFactory>();
        }

        private Color GetNextColor()
        {
            _colorValue++;
            return Color.FromArgb(_colorValue, _colorValue, _colorValue, _colorValue);
        }
    }
}