using System.Collections.Generic;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class TerritoryLayoutInformationFactory : ITerritoryLayoutInformationFactory
    {
        private readonly Dictionary<ILocation, ITerritoryLayoutInformation> _layoutInformation;

        public TerritoryLayoutInformationFactory(ILocationRepository locationRepository)
        {
            _layoutInformation = new Dictionary<ILocation, ITerritoryLayoutInformation>
                {
                    { locationRepository.Alaska, new AlaskaLayoutInformation() },
                    { locationRepository.Alberta, new AlbertaLayoutInformation() },
                    { locationRepository.CentralAmerica, new CentralAmericaLayoutInformation() },
                    { locationRepository.EasternUnitedStates, new EasternUnitedStatesLayoutInformation() },
                    { locationRepository.Greenland, new GreenlandLayoutInformation() },
                    { locationRepository.NorthwestTerritory, new NorthwestTerritoryLayoutInformation() },
                    { locationRepository.Ontario, new OntarioLayoutInformation() },
                    { locationRepository.Quebec, new QuebecLayoutInformation() },
                    { locationRepository.WesternUnitedStates, new WesternUnitedStatesLayoutInformation() },
                    { locationRepository.Argentina, new ArgentinaLayoutInformation() },
                    { locationRepository.Brazil, new BrazilViewModelsFactory() },
                    { locationRepository.Peru, new PeruLayoutInformation() },
                    { locationRepository.Venezuela, new VenezuelaLayoutInformation() },
                    { locationRepository.GreatBritain, new GreatBritainLayoutInformation() },
                    { locationRepository.Iceland, new IcelandLayoutInformation() },
                    { locationRepository.NorthernEurope, new NorthernEuropeLayoutInformation() },
                    { locationRepository.Scandinavia, new ScandinaviaLayoutInformation() },
                    { locationRepository.SouthernEurope, new SouthernEuropeLayoutInformation() },
                    { locationRepository.Ukraine, new UkraineLayoutInformation() },
                    { locationRepository.WesternEurope, new WesternEuropeLayoutInformation() },
                    { locationRepository.Congo, new CongoLayoutInformation() },
                    { locationRepository.EastAfrica, new EastAfricaLayoutInformation() },
                    { locationRepository.Egypt, new EgyptLayoutInformation() },
                    { locationRepository.Madagascar, new MadagascarLayoutInformation() },
                    { locationRepository.NorthAfrica, new NorthAfricaLayoutInformation() },
                    { locationRepository.SouthAfrica, new SouthAfricaLayoutInformation() },
                    { locationRepository.Afghanistan, new AfhanistanLayoutInformation() },
                    { locationRepository.China, new ChinaLayoutInformation() },
                    { locationRepository.India, new IndiaLayoutInformation() },
                    { locationRepository.Irkutsk, new IrkutskLayoutInformation() },
                    { locationRepository.Japan, new JapanLayoutInformation() },
                    { locationRepository.Kamchatka, new KamchatkaLayoutInformation() },
                    { locationRepository.MiddleEast, new MiddleEastLayoutInformation() },
                    { locationRepository.Mongolia, new MongoliaLayoutInformation() },
                    { locationRepository.Siam, new SiamLayoutInformation() },
                    { locationRepository.Siberia, new SiberiaLayoutInformation() },
                    { locationRepository.Ural, new UralLayoutInformation() },
                    { locationRepository.Yakutsk, new YakutskLayoutInformation() },
                    { locationRepository.EasternAustralia, new EasternAustraliaLayoutInformation() },
                    { locationRepository.Indonesia, new IndonesiaLayoutInformation() },
                    { locationRepository.NewGuinea, new NewGuineaLayoutInformation() },
                    { locationRepository.WesternAustralia, new WesternAustraliaLayoutInformation() },
                };
        }

        public ITerritoryLayoutInformation Create(ILocation location)
        {
            return _layoutInformation[location];
        }
    }
}