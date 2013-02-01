using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Caliburn.Micro;
using RISK.Services;

namespace RISK.WorldMap.Territories
{
    public class TerritoryViewModelFactory : ITerritoryViewModelFactory
    {
        private readonly IColorService _colorService;

        public TerritoryViewModelFactory(IColorService colorService)
        {
            _colorService = colorService;
        }

        public IEnumerable<TerritoryViewModelBase> Create()
        {
            return CreateNorthAmericaTerritories()
                //.Join(CreateSouthAmericaTerritories)
                //.Join(CreateAfricaTerritories)
                //.Join(CreateEuropeTerritories)
                //.Join(CreateAsiaTerritories)
                //.Join(CreateAustraliaTerritories)
                .ToList();
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
            var northAmericaColors = _colorService.GetNorthAmericaColors();

            territoryViewModel.NormalStrokeColor = northAmericaColors.NormalStrokeColor;
            territoryViewModel.NormalFillColor = northAmericaColors.NormalFillColor;
            territoryViewModel.MouseOverStrokeColor = northAmericaColors.MouseOverStrokeColor;
            territoryViewModel.MouseOverFillColor = northAmericaColors.MouseOverFillColor;
        }
    }
}