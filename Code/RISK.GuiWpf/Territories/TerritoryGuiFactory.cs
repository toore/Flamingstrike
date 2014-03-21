using System.Collections.Generic;
using RISK.Domain;
using RISK.Domain.Entities;

namespace GuiWpf.Territories
{
    public class TerritoryGuiFactory : ITerritoryGuiFactory
    {
        private readonly Dictionary<ILocation, ITerritoryGraphics> _layoutInformation;

        public TerritoryGuiFactory(Locations locations)
        {
            _layoutInformation = new Dictionary<ILocation, ITerritoryGraphics>
            {
                { locations.Alaska, new AlaskaGraphics() },
                { locations.Alberta, new AlbertaGraphics() },
                { locations.CentralAmerica, new CentralAmericaGraphics() },
                { locations.EasternUnitedStates, new EasternUnitedStatesGraphics() },
                { locations.Greenland, new GreenlandGraphics() },
                { locations.NorthwestTerritory, new NorthwestTerritoryGraphics() },
                { locations.Ontario, new OntarioGraphics() },
                { locations.Quebec, new QuebecGraphics() },
                { locations.WesternUnitedStates, new WesternUnitedStatesGraphics() },
                { locations.Argentina, new ArgentinaGraphics() },
                { locations.Brazil, new BrazilGraphics() },
                { locations.Peru, new PeruGraphics() },
                { locations.Venezuela, new VenezuelaGraphics() },
                { locations.GreatBritain, new GreatBritainGraphics() },
                { locations.Iceland, new IcelandGraphics() },
                { locations.NorthernEurope, new NorthernEuropeGraphics() },
                { locations.Scandinavia, new ScandinaviaGraphics() },
                { locations.SouthernEurope, new SouthernEuropeGraphics() },
                { locations.Ukraine, new UkraineGraphics() },
                { locations.WesternEurope, new WesternEuropeGraphics() },
                { locations.Congo, new CongoGraphics() },
                { locations.EastAfrica, new EastAfricaGraphics() },
                { locations.Egypt, new EgyptGraphics() },
                { locations.Madagascar, new MadagascarGraphics() },
                { locations.NorthAfrica, new NorthAfricaGraphics() },
                { locations.SouthAfrica, new SouthAfricaGraphics() },
                { locations.Afghanistan, new AfhanistanGraphics() },
                { locations.China, new ChinaGraphics() },
                { locations.India, new IndiaGraphics() },
                { locations.Irkutsk, new IrkutskGraphics() },
                { locations.Japan, new JapanGraphics() },
                { locations.Kamchatka, new KamchatkaGraphics() },
                { locations.MiddleEast, new MiddleEastGraphics() },
                { locations.Mongolia, new MongoliaGraphics() },
                { locations.Siam, new SiamGraphics() },
                { locations.Siberia, new SiberiaGraphics() },
                { locations.Ural, new UralGraphics() },
                { locations.Yakutsk, new YakutskGraphics() },
                { locations.EasternAustralia, new EasternAustraliaGraphics() },
                { locations.Indonesia, new IndonesiaGraphics() },
                { locations.NewGuinea, new NewGuineaGraphics() },
                { locations.WesternAustralia, new WesternAustraliaGraphics() },
            };
        }

        public ITerritoryGraphics Create(ILocation location)
        {
            return _layoutInformation[location];
        }
    }
}