using System.Collections.Generic;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.Territories
{
    public class TerritoryGuiFactory : ITerritoryGuiFactory
    {
        private readonly Dictionary<ILocation, ITerritoryGui> _layoutInformation;

        public TerritoryGuiFactory(ILocationProvider locationProvider)
        {
            _layoutInformation = new Dictionary<ILocation, ITerritoryGui>
                {
                    { locationProvider.Alaska, new AlaskaGui() },
                    { locationProvider.Alberta, new AlbertaGui() },
                    { locationProvider.CentralAmerica, new CentralAmericaGui() },
                    { locationProvider.EasternUnitedStates, new EasternUnitedStatesGui() },
                    { locationProvider.Greenland, new GreenlandGui() },
                    { locationProvider.NorthwestTerritory, new NorthwestTerritoryGui() },
                    { locationProvider.Ontario, new OntarioGui() },
                    { locationProvider.Quebec, new QuebecGui() },
                    { locationProvider.WesternUnitedStates, new WesternUnitedStatesGui() },
                    { locationProvider.Argentina, new ArgentinaGui() },
                    { locationProvider.Brazil, new BrazilViewModelsFactory() },
                    { locationProvider.Peru, new PeruGui() },
                    { locationProvider.Venezuela, new VenezuelaGui() },
                    { locationProvider.GreatBritain, new GreatBritainGui() },
                    { locationProvider.Iceland, new IcelandGui() },
                    { locationProvider.NorthernEurope, new NorthernEuropeGui() },
                    { locationProvider.Scandinavia, new ScandinaviaGui() },
                    { locationProvider.SouthernEurope, new SouthernEuropeGui() },
                    { locationProvider.Ukraine, new UkraineGui() },
                    { locationProvider.WesternEurope, new WesternEuropeGui() },
                    { locationProvider.Congo, new CongoGui() },
                    { locationProvider.EastAfrica, new EastAfricaGui() },
                    { locationProvider.Egypt, new EgyptGui() },
                    { locationProvider.Madagascar, new MadagascarGui() },
                    { locationProvider.NorthAfrica, new NorthAfricaGui() },
                    { locationProvider.SouthAfrica, new SouthAfricaGui() },
                    { locationProvider.Afghanistan, new AfhanistanGui() },
                    { locationProvider.China, new ChinaGui() },
                    { locationProvider.India, new IndiaGui() },
                    { locationProvider.Irkutsk, new IrkutskGui() },
                    { locationProvider.Japan, new JapanGui() },
                    { locationProvider.Kamchatka, new KamchatkaGui() },
                    { locationProvider.MiddleEast, new MiddleEastGui() },
                    { locationProvider.Mongolia, new MongoliaGui() },
                    { locationProvider.Siam, new SiamGui() },
                    { locationProvider.Siberia, new SiberiaGui() },
                    { locationProvider.Ural, new UralGui() },
                    { locationProvider.Yakutsk, new YakutskGui() },
                    { locationProvider.EasternAustralia, new EasternAustraliaGui() },
                    { locationProvider.Indonesia, new IndonesiaGui() },
                    { locationProvider.NewGuinea, new NewGuineaGui() },
                    { locationProvider.WesternAustralia, new WesternAustraliaGui() },
                };
        }

        public ITerritoryGui Create(ILocation location)
        {
            return _layoutInformation[location];
        }
    }
}