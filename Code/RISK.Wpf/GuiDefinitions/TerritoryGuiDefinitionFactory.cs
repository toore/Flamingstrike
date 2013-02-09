using System.Collections.Generic;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.GuiDefinitions
{
    public class TerritoryGuiDefinitionFactory : ITerritoryGuiDefinitionFactory
    {
        private readonly Dictionary<ILocation, ITerritoryGuiDefinitions> _layoutInformation;

        public TerritoryGuiDefinitionFactory(ILocationRepository locationRepository)
        {
            _layoutInformation = new Dictionary<ILocation, ITerritoryGuiDefinitions>
                {
                    { locationRepository.Alaska, new AlaskaGuiDefinitions() },
                    { locationRepository.Alberta, new AlbertaGuiDefinitions() },
                    { locationRepository.CentralAmerica, new CentralAmericaGuiDefinitions() },
                    { locationRepository.EasternUnitedStates, new EasternUnitedStatesGuiDefinitions() },
                    { locationRepository.Greenland, new GreenlandGuiDefinitions() },
                    { locationRepository.NorthwestTerritory, new NorthwestTerritoryGuiDefinitions() },
                    { locationRepository.Ontario, new OntarioGuiDefinitions() },
                    { locationRepository.Quebec, new QuebecGuiDefinitions() },
                    { locationRepository.WesternUnitedStates, new WesternUnitedStatesGuiDefinitions() },
                    { locationRepository.Argentina, new ArgentinaGuiDefinitions() },
                    { locationRepository.Brazil, new BrazilViewModelsFactory() },
                    { locationRepository.Peru, new PeruGuiDefinitions() },
                    { locationRepository.Venezuela, new VenezuelaGuiDefinitions() },
                    { locationRepository.GreatBritain, new GreatBritainGuiDefinitions() },
                    { locationRepository.Iceland, new IcelandGuiDefinitions() },
                    { locationRepository.NorthernEurope, new NorthernEuropeGuiDefinitions() },
                    { locationRepository.Scandinavia, new ScandinaviaGuiDefinitions() },
                    { locationRepository.SouthernEurope, new SouthernEuropeGuiDefinitions() },
                    { locationRepository.Ukraine, new UkraineGuiDefinitions() },
                    { locationRepository.WesternEurope, new WesternEuropeGuiDefinitions() },
                    { locationRepository.Congo, new CongoGuiDefinitions() },
                    { locationRepository.EastAfrica, new EastAfricaGuiDefinitions() },
                    { locationRepository.Egypt, new EgyptGuiDefinitions() },
                    { locationRepository.Madagascar, new MadagascarGuiDefinitions() },
                    { locationRepository.NorthAfrica, new NorthAfricaGuiDefinitions() },
                    { locationRepository.SouthAfrica, new SouthAfricaGuiDefinitions() },
                    { locationRepository.Afghanistan, new AfhanistanGuiDefinitions() },
                    { locationRepository.China, new ChinaGuiDefinitions() },
                    { locationRepository.India, new IndiaGuiDefinitions() },
                    { locationRepository.Irkutsk, new IrkutskGuiDefinitions() },
                    { locationRepository.Japan, new JapanGuiDefinitions() },
                    { locationRepository.Kamchatka, new KamchatkaGuiDefinitions() },
                    { locationRepository.MiddleEast, new MiddleEastGuiDefinitions() },
                    { locationRepository.Mongolia, new MongoliaGuiDefinitions() },
                    { locationRepository.Siam, new SiamGuiDefinitions() },
                    { locationRepository.Siberia, new SiberiaGuiDefinitions() },
                    { locationRepository.Ural, new UralGuiDefinitions() },
                    { locationRepository.Yakutsk, new YakutskGuiDefinitions() },
                    { locationRepository.EasternAustralia, new EasternAustraliaGuiDefinitions() },
                    { locationRepository.Indonesia, new IndonesiaGuiDefinitions() },
                    { locationRepository.NewGuinea, new NewGuineaGuiDefinitions() },
                    { locationRepository.WesternAustralia, new WesternAustraliaGuiDefinitions() },
                };
        }

        public ITerritoryGuiDefinitions Create(ILocation location)
        {
            return _layoutInformation[location];
        }
    }
}