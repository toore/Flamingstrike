using System.Windows.Media;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.Views.WorldMapView.Territories;
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
        private IColorService _colorService;
        private ContinentColors _asiaColors;
        private byte _colorValue;

        [SetUp]
        public void SetUp()
        {
            _continentRepository = new ContinentRepository();
            _locationRepository = new LocationRepository(_continentRepository);
            _colorService = MockRepository.GenerateStub<IColorService>();

            _asiaColors = new ContinentColors(GetNextColor(), GetNextColor(), GetNextColor(), GetNextColor());
            _colorService.Stub(x => x.AsiaColors).Return(_asiaColors);

            _territoryViewModelFactory = new TerritoryViewModelFactory(_locationRepository, _continentRepository, _colorService);
        }

        [Test]
        public void Create_Siam_view_model_with_default_colors()
        {
            var territory = MockRepository.GenerateStub<ITerritory>();
            territory.Stub(x => x.Location).Return(_locationRepository.Siam);

            var viewModel = _territoryViewModelFactory.Create(territory);

            viewModel.NormalStrokeColor.Should().Be(_asiaColors.NormalStrokeColor);
            viewModel.NormalFillColor.Should().Be(_asiaColors.NormalFillColor);
            viewModel.MouseOverStrokeColor.Should().Be(_asiaColors.MouseOverStrokeColor);
            viewModel.MouseOverFillColor.Should().Be(_asiaColors.MouseOverFillColor);
        }

        private Color GetNextColor()
        {
            _colorValue++;
            return Color.FromArgb(_colorValue, _colorValue, _colorValue, _colorValue);
        }
    }
}