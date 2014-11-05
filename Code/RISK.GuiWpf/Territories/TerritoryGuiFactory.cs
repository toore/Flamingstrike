using System.Collections.Generic;
using RISK.Application.Entities;

namespace GuiWpf.Territories
{
    public class TerritoryGuiFactory : ITerritoryGuiFactory
    {
        private readonly Dictionary<ITerritory, ITerritoryGraphics> _layoutInformation;

        public TerritoryGuiFactory(RISK.Application.WorldMap worldMap)
        {
            _layoutInformation = new Dictionary<ITerritory, ITerritoryGraphics>
            {
                { worldMap.Alaska, new AlaskaGraphics() },
                { worldMap.Alberta, new AlbertaGraphics() },
                { worldMap.CentralAmerica, new CentralAmericaGraphics() },
                { worldMap.EasternUnitedStates, new EasternUnitedStatesGraphics() },
                { worldMap.Greenland, new GreenlandGraphics() },
                { worldMap.NorthwestTerritory, new NorthwestTerritoryGraphics() },
                { worldMap.Ontario, new OntarioGraphics() },
                { worldMap.Quebec, new QuebecGraphics() },
                { worldMap.WesternUnitedStates, new WesternUnitedStatesGraphics() },
                { worldMap.Argentina, new ArgentinaGraphics() },
                { worldMap.Brazil, new BrazilGraphics() },
                { worldMap.Peru, new PeruGraphics() },
                { worldMap.Venezuela, new VenezuelaGraphics() },
                { worldMap.GreatBritain, new GreatBritainGraphics() },
                { worldMap.Iceland, new IcelandGraphics() },
                { worldMap.NorthernEurope, new NorthernEuropeGraphics() },
                { worldMap.Scandinavia, new ScandinaviaGraphics() },
                { worldMap.SouthernEurope, new SouthernEuropeGraphics() },
                { worldMap.Ukraine, new UkraineGraphics() },
                { worldMap.WesternEurope, new WesternEuropeGraphics() },
                { worldMap.Congo, new CongoGraphics() },
                { worldMap.EastAfrica, new EastAfricaGraphics() },
                { worldMap.Egypt, new EgyptGraphics() },
                { worldMap.Madagascar, new MadagascarGraphics() },
                { worldMap.NorthAfrica, new NorthAfricaGraphics() },
                { worldMap.SouthAfrica, new SouthAfricaGraphics() },
                { worldMap.Afghanistan, new AfhanistanGraphics() },
                { worldMap.China, new ChinaGraphics() },
                { worldMap.India, new IndiaGraphics() },
                { worldMap.Irkutsk, new IrkutskGraphics() },
                { worldMap.Japan, new JapanGraphics() },
                { worldMap.Kamchatka, new KamchatkaGraphics() },
                { worldMap.MiddleEast, new MiddleEastGraphics() },
                { worldMap.Mongolia, new MongoliaGraphics() },
                { worldMap.Siam, new SiamGraphics() },
                { worldMap.Siberia, new SiberiaGraphics() },
                { worldMap.Ural, new UralGraphics() },
                { worldMap.Yakutsk, new YakutskGraphics() },
                { worldMap.EasternAustralia, new EasternAustraliaGraphics() },
                { worldMap.Indonesia, new IndonesiaGraphics() },
                { worldMap.NewGuinea, new NewGuineaGraphics() },
                { worldMap.WesternAustralia, new WesternAustraliaGraphics() },
            };
        }

        public ITerritoryGraphics Create(ITerritory location)
        {
            return _layoutInformation[location];
        }
    }
}