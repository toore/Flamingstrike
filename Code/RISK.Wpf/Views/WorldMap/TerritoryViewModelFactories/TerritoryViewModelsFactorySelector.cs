using System.Collections.Generic;
using GuiWpf.Services;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class TerritoryViewModelsFactorySelector : ITerritoryViewModelsFactorySelector
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IDictionary<ILocation, ITerritoryViewModelsFactory> _factories;

        public TerritoryViewModelsFactorySelector(ILocationRepository locationRepository, IColorService colorService)
        {
            _locationRepository = locationRepository;

            _factories = new Dictionary<ILocation, ITerritoryViewModelsFactory>
                {
                    { _locationRepository.Alaska, new AlaskaViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.Alberta, new AlbertaViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.CentralAmerica, new CentralAmericaViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.EasternUnitedStates, new EasternUnitedStatesViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.Greenland, new GreenlandViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.NorthwestTerritory, new NorthwestTerritoryViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.Ontario, new OntarioViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.Quebec, new QuebecViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.WesternUnitedStates, new WesternUnitedStatesViewModelsFactory(colorService.NorthAmericaColors) },
                    { _locationRepository.Argentina, new ArgentinaViewModelsFactory(colorService.SouthAmericaColors) },
                    { _locationRepository.Brazil, new BrazilViewModelsFactory(colorService.SouthAmericaColors) },
                    { _locationRepository.Peru, new PeruViewModelsFactory(colorService.SouthAmericaColors) },
                    { _locationRepository.Venezuela, new VenezuelaViewModelsFactory(colorService.SouthAmericaColors) },
                    { _locationRepository.GreatBritain, new GreatBritainViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.Iceland, new IcelandViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.NorthernEurope, new NorthernEuropeViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.Scandinavia, new ScandinaviaViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.SouthernEurope, new SouthernEuropeViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.Ukraine, new UkraineViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.WesternEurope, new WesternEuropeViewModelsFactory(colorService.EuropeColors) },
                    { _locationRepository.Congo, new CongoViewModelsFactory(colorService.AfricaColors) },
                    { _locationRepository.EastAfrica, new EastAfricaViewModelsFactory(colorService.AfricaColors) },
                    { _locationRepository.Egypt, new EgyptViewModelsFactory(colorService.AfricaColors) },
                    { _locationRepository.Madagascar, new MadagascarViewModelsFactory(colorService.AfricaColors) },
                    { _locationRepository.NorthAfrica, new NorthAfricaViewModelsFactory(colorService.AfricaColors) },
                    { _locationRepository.SouthAfrica, new SouthAfricaViewModelsFactory(colorService.AfricaColors) },
                    { _locationRepository.Afghanistan, new AfhanistanViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.China, new ChinaViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.India, new IndiaViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Irkutsk, new IrkutskViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Japan, new JapanViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Kamchatka, new KamchatkaViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.MiddleEast, new MiddleEastViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Mongolia, new MongoliaViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Siam, new SiamViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Siberia, new SiberiaViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Ural, new UralViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.Yakutsk, new YakutskViewModelsFactory(colorService.AsiaColors) },
                    { _locationRepository.EasternAustralia, new EasternAustraliaViewModel(colorService.AustraliaColors) },
                    { _locationRepository.Indonesia, new IndonesiaViewModelsFactory(colorService.AustraliaColors) },
                    { _locationRepository.NewGuinea, new NewGuineaViewModelsFactory(colorService.AustraliaColors) },
                    { _locationRepository.WesternAustralia, new WesternAustraliaViewModelsFactory(colorService.AustraliaColors) },
                };
        }

        public ITerritoryViewModelsFactory Select(ITerritory territory)
        {
            return _factories[territory.Location];
        }
    }
}