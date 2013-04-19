using System.Collections.Generic;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.GuiDefinitions
{
    public class TerritoryGuiDefinitionFactory : ITerritoryGuiDefinitionFactory
    {
        private readonly Dictionary<ILocation, ITerritoryGuiDefinitions> _layoutInformation;

        public TerritoryGuiDefinitionFactory(ILocationProvider locationProvider)
        {
            _layoutInformation = new Dictionary<ILocation, ITerritoryGuiDefinitions>
                {
                    { locationProvider.Alaska, new AlaskaGuiDefinitions() },
                    { locationProvider.Alberta, new AlbertaGuiDefinitions() },
                    { locationProvider.CentralAmerica, new CentralAmericaGuiDefinitions() },
                    { locationProvider.EasternUnitedStates, new EasternUnitedStatesGuiDefinitions() },
                    { locationProvider.Greenland, new GreenlandGuiDefinitions() },
                    { locationProvider.NorthwestTerritory, new NorthwestTerritoryGuiDefinitions() },
                    { locationProvider.Ontario, new OntarioGuiDefinitions() },
                    { locationProvider.Quebec, new QuebecGuiDefinitions() },
                    { locationProvider.WesternUnitedStates, new WesternUnitedStatesGuiDefinitions() },
                    { locationProvider.Argentina, new ArgentinaGuiDefinitions() },
                    { locationProvider.Brazil, new BrazilViewModelsFactory() },
                    { locationProvider.Peru, new PeruGuiDefinitions() },
                    { locationProvider.Venezuela, new VenezuelaGuiDefinitions() },
                    { locationProvider.GreatBritain, new GreatBritainGuiDefinitions() },
                    { locationProvider.Iceland, new IcelandGuiDefinitions() },
                    { locationProvider.NorthernEurope, new NorthernEuropeGuiDefinitions() },
                    { locationProvider.Scandinavia, new ScandinaviaGuiDefinitions() },
                    { locationProvider.SouthernEurope, new SouthernEuropeGuiDefinitions() },
                    { locationProvider.Ukraine, new UkraineGuiDefinitions() },
                    { locationProvider.WesternEurope, new WesternEuropeGuiDefinitions() },
                    { locationProvider.Congo, new CongoGuiDefinitions() },
                    { locationProvider.EastAfrica, new EastAfricaGuiDefinitions() },
                    { locationProvider.Egypt, new EgyptGuiDefinitions() },
                    { locationProvider.Madagascar, new MadagascarGuiDefinitions() },
                    { locationProvider.NorthAfrica, new NorthAfricaGuiDefinitions() },
                    { locationProvider.SouthAfrica, new SouthAfricaGuiDefinitions() },
                    { locationProvider.Afghanistan, new AfhanistanGuiDefinitions() },
                    { locationProvider.China, new ChinaGuiDefinitions() },
                    { locationProvider.India, new IndiaGuiDefinitions() },
                    { locationProvider.Irkutsk, new IrkutskGuiDefinitions() },
                    { locationProvider.Japan, new JapanGuiDefinitions() },
                    { locationProvider.Kamchatka, new KamchatkaGuiDefinitions() },
                    { locationProvider.MiddleEast, new MiddleEastGuiDefinitions() },
                    { locationProvider.Mongolia, new MongoliaGuiDefinitions() },
                    { locationProvider.Siam, new SiamGuiDefinitions() },
                    { locationProvider.Siberia, new SiberiaGuiDefinitions() },
                    { locationProvider.Ural, new UralGuiDefinitions() },
                    { locationProvider.Yakutsk, new YakutskGuiDefinitions() },
                    { locationProvider.EasternAustralia, new EasternAustraliaGuiDefinitions() },
                    { locationProvider.Indonesia, new IndonesiaGuiDefinitions() },
                    { locationProvider.NewGuinea, new NewGuineaGuiDefinitions() },
                    { locationProvider.WesternAustralia, new WesternAustraliaGuiDefinitions() },
                };
        }

        public ITerritoryGuiDefinitions Create(ILocation location)
        {
            return _layoutInformation[location];
        }
    }
}