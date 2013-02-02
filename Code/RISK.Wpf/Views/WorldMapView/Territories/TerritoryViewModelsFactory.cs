using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Domain.Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class TerritoryViewModelsFactory : ITerritoryViewModelsFactory
    {
        private readonly ITerritoryViewModelFactory _territoryViewModelFactory;
        private readonly IWorldMap _worldMap;
        private readonly ILocationRepository _locationRepository;
        private readonly IColorService _colorService;

        public TerritoryViewModelsFactory(ITerritoryViewModelFactory territoryViewModelFactory, IWorldMap worldMap, ILocationRepository locationRepository, IColorService colorService)
        {
            _territoryViewModelFactory = territoryViewModelFactory;
            _worldMap = worldMap;
            _locationRepository = locationRepository;
            _colorService = colorService;
        }

        public IEnumerable<TerritoryViewModelBase> Create()
        {
            return _locationRepository.GetAll()
                .Select(_worldMap.GetTerritory)
                .Select(CreateViewModel)
                .ToList();

            //return CreateNorthAmericaTerritories()
            //    //.Join(CreateSouthAmericaTerritories)
            //    //.Join(CreateAfricaTerritories)
            //    //.Join(CreateEuropeTerritories)
            //    //.Join(CreateAsiaTerritories)
            //    //.Join(CreateAustraliaTerritories)
            //    .ToList();
        }

        private TerritoryViewModelBase CreateViewModel(ITerritory territory)
        {
            var viewModel = _territoryViewModelFactory.Create(territory);

            return viewModel;
        }

        private IEnumerable<TerritoryViewModelBase> CreateNorthAmericaTerritories()
        {
            var territories = new TerritoryViewModelBase[]
                {
                    new AlaskaViewModel(),
                    new AlbertaViewModel(),
                    new CentralAmericaViewModel(),
                    new EasternUnitedStatesViewModel(),
                    new GreenlandViewModel(),
                    new NorthwestTerritoryViewModel(),
                    new OntarioViewModel(),
                    new QuebecViewModel(),
                    new WesternUnitedStatesViewModel()
                };

            territories.Apply(SetNorthAmericaColors);

            return territories;
        }

        private void SetNorthAmericaColors(TerritoryViewModelBase territoryViewModel)
        {
            var northAmericaColors = _colorService.NorthAmericaColors;

            territoryViewModel.NormalStrokeColor = northAmericaColors.NormalStrokeColor;
            territoryViewModel.NormalFillColor = northAmericaColors.NormalFillColor;
            territoryViewModel.MouseOverStrokeColor = northAmericaColors.MouseOverStrokeColor;
            territoryViewModel.MouseOverFillColor = northAmericaColors.MouseOverFillColor;
        }
    }
}