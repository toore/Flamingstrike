using System;
using System.Collections.Generic;
using GuiWpf.Services;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class TerritoryViewModelFactory : ITerritoryViewModelFactory
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IContinentRepository _continentRepository;
        private readonly IColorService _colorService;
        private readonly IDictionary<ILocation, Func<TerritoryViewModelBase>> _viewModelCreator;
        private readonly Dictionary<Continent, Func<ContinentColors>> _continentColors;
        private TerritoryViewModelBase _viewModel;

        public TerritoryViewModelFactory(ILocationRepository locationRepository, IContinentRepository continentRepository, IColorService colorService)
        {
            _locationRepository = locationRepository;
            _continentRepository = continentRepository;
            _colorService = colorService;

            _viewModelCreator = new Dictionary<ILocation, Func<TerritoryViewModelBase>>
                {
                    { _locationRepository.Alaska, () => new AlaskaViewModel() },
                    { _locationRepository.Alberta, () => new AlbertaViewModel() },
                    { _locationRepository.CentralAmerica, () => new CentralAmericaViewModel() },
                    { _locationRepository.EasternUnitedStates, () => new EasternUnitedStatesViewModel() },
                    { _locationRepository.Greenland, () => new GreenlandViewModel() },
                    { _locationRepository.Northwest, () => new NorthwestTerritoryViewModel() },
                    { _locationRepository.Ontario, () => new OntarioViewModel() },
                    { _locationRepository.Quebec, () => new QuebecViewModel() },
                    { _locationRepository.WesternUnitedStates, () => new WesternUnitedStatesViewModel() },
                    { _locationRepository.Argentina, () => new ArgentinaViewModel() },
                    { _locationRepository.Brazil, () => new BrazilViewModel() },
                    { _locationRepository.Peru, () => new PeruViewModel() },
                    { _locationRepository.Venezuela, () => new VenezuelaViewModel() },
                    { _locationRepository.GreatBritain, () => new GreatBritainViewModel() },
                    { _locationRepository.Iceland, () => new IcelandViewModel() },
                    { _locationRepository.NorthernEurope, () => new NorthernEuropeViewModel() },
                    { _locationRepository.Scandinavia, () => new ScandinaviaViewModel() },
                    { _locationRepository.SouthernEurope, () => new SouthernEuropeViewModel() },
                    { _locationRepository.Ukraine, () => new UkraineViewModel() },
                    { _locationRepository.WesternEurope, () => new WesternEuropeViewModel() },
                    { _locationRepository.Congo, () => new CongoViewModel() },
                    { _locationRepository.EastAfrica, () => new EastAfricaViewModel() },
                    { _locationRepository.Egypt, () => new EgyptViewModel() },
                    { _locationRepository.Madagascar, () => new MadagascarViewModel() },
                    { _locationRepository.NorthAfrica, () => new NorthAfricaViewModel() },
                    { _locationRepository.SouthAfrica, () => new SouthAfricaViewModel() },
                    { _locationRepository.Afghanistan, () => new AfhanistanViewModel() },
                    { _locationRepository.China, () => new ChinaViewModel() },
                    { _locationRepository.India, () => new IndiaViewModel() },
                    { _locationRepository.Irkutsk, () => new IrkutskViewModel() },
                    { _locationRepository.Japan, () => new JapanViewModel() },
                    { _locationRepository.Kamchatka, () => new KamchatkaViewModel() },
                    { _locationRepository.MiddleEast, () => new MiddleEastViewModel() },
                    { _locationRepository.Mongolia, () => new MongoliaViewModel() },
                    { _locationRepository.Siam, () => new SiamViewModel() },
                    { _locationRepository.Siberia, () => new SiberiaViewModel() },
                    { _locationRepository.Ural, () => new UralViewModel() },
                    { _locationRepository.Yakutsk, () => new YakutskViewModel() },
                    { _locationRepository.EasternAustralia, () => new EasternAustraliaViewModel() },
                    { _locationRepository.Indonesia, () => new IndonesiaViewModel() },
                    { _locationRepository.NewGuinea, () => new NewGuineaViewModel() },
                    { _locationRepository.WesternAustralia, () => new WesternAustraliaViewModel() },
                };

            _continentColors = new Dictionary<Continent, Func<ContinentColors>>
                {
                    { _continentRepository.NorthAmerica, () => _colorService.NorthAmericaColors },
                    { _continentRepository.SouthAmerica, () => _colorService.SouthAmericaColors },
                    { _continentRepository.Europe, () => _colorService.EuropeColors },
                    { _continentRepository.Africa, () => _colorService.AfricaColors },
                    { _continentRepository.Asia, () => _colorService.AsiaColors },
                    { _continentRepository.Australia, () => _colorService.AustraliaColors },
                };
        }

        public TerritoryViewModelBase Create(ITerritory territory)
        {
            _viewModel = _viewModelCreator[territory.Location]();

            if (!territory.HasOwner)
            {
                SetDefaultColors(territory.Location);
            }
            else
            {
                // TODO: Set owner player color
            }

            return _viewModel;
        }

        private void SetDefaultColors(ILocation location)
        {
            var continentColors = _continentColors[location.Continent]();

            _viewModel.NormalStrokeColor = continentColors.NormalStrokeColor;
            _viewModel.NormalFillColor = continentColors.NormalFillColor;
            _viewModel.MouseOverStrokeColor = continentColors.MouseOverStrokeColor;
            _viewModel.MouseOverFillColor = continentColors.MouseOverFillColor;
        }
    }
}