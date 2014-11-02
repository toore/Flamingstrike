using System.Collections.Generic;
using RISK.Domain;
using RISK.Domain.Entities;

namespace GuiWpf.Territories
{
    public class TerritoryGuiFactory : ITerritoryGuiFactory
    {
        private readonly Dictionary<ITerritory, ITerritoryGraphics> _layoutInformation;

        public TerritoryGuiFactory(RISK.Domain.Territories territories)
        {
            _layoutInformation = new Dictionary<ITerritory, ITerritoryGraphics>
            {
                { territories.Alaska, new AlaskaGraphics() },
                { territories.Alberta, new AlbertaGraphics() },
                { territories.CentralAmerica, new CentralAmericaGraphics() },
                { territories.EasternUnitedStates, new EasternUnitedStatesGraphics() },
                { territories.Greenland, new GreenlandGraphics() },
                { territories.NorthwestTerritory, new NorthwestTerritoryGraphics() },
                { territories.Ontario, new OntarioGraphics() },
                { territories.Quebec, new QuebecGraphics() },
                { territories.WesternUnitedStates, new WesternUnitedStatesGraphics() },
                { territories.Argentina, new ArgentinaGraphics() },
                { territories.Brazil, new BrazilGraphics() },
                { territories.Peru, new PeruGraphics() },
                { territories.Venezuela, new VenezuelaGraphics() },
                { territories.GreatBritain, new GreatBritainGraphics() },
                { territories.Iceland, new IcelandGraphics() },
                { territories.NorthernEurope, new NorthernEuropeGraphics() },
                { territories.Scandinavia, new ScandinaviaGraphics() },
                { territories.SouthernEurope, new SouthernEuropeGraphics() },
                { territories.Ukraine, new UkraineGraphics() },
                { territories.WesternEurope, new WesternEuropeGraphics() },
                { territories.Congo, new CongoGraphics() },
                { territories.EastAfrica, new EastAfricaGraphics() },
                { territories.Egypt, new EgyptGraphics() },
                { territories.Madagascar, new MadagascarGraphics() },
                { territories.NorthAfrica, new NorthAfricaGraphics() },
                { territories.SouthAfrica, new SouthAfricaGraphics() },
                { territories.Afghanistan, new AfhanistanGraphics() },
                { territories.China, new ChinaGraphics() },
                { territories.India, new IndiaGraphics() },
                { territories.Irkutsk, new IrkutskGraphics() },
                { territories.Japan, new JapanGraphics() },
                { territories.Kamchatka, new KamchatkaGraphics() },
                { territories.MiddleEast, new MiddleEastGraphics() },
                { territories.Mongolia, new MongoliaGraphics() },
                { territories.Siam, new SiamGraphics() },
                { territories.Siberia, new SiberiaGraphics() },
                { territories.Ural, new UralGraphics() },
                { territories.Yakutsk, new YakutskGraphics() },
                { territories.EasternAustralia, new EasternAustraliaGraphics() },
                { territories.Indonesia, new IndonesiaGraphics() },
                { territories.NewGuinea, new NewGuineaGraphics() },
                { territories.WesternAustralia, new WesternAustraliaGraphics() },
            };
        }

        public ITerritoryGraphics Create(ITerritory location)
        {
            return _layoutInformation[location];
        }
    }
}