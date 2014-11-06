using System.Collections.Generic;
using RISK.Application;
using RISK.Application.Entities;

namespace GuiWpf.TerritoryModels
{
    public interface ITerritoryGuiFactory
    {
        ITerritoryModel Create(ITerritory territory);
    }

    public class TerritoryGuiFactory : ITerritoryGuiFactory
    {
        private readonly Dictionary<ITerritory, ITerritoryModel> _layoutInformation;

        public TerritoryGuiFactory(WorldMap worldMap)
        {
            _layoutInformation = new Dictionary<ITerritory, ITerritoryModel>
            {
                { worldMap.Alaska, new AlaskaModel() },
                { worldMap.Alberta, new AlbertaModel() },
                { worldMap.CentralAmerica, new CentralAmericaModel() },
                { worldMap.EasternUnitedStates, new EasternUnitedStatesModel() },
                { worldMap.Greenland, new GreenlandModel() },
                { worldMap.NorthwestTerritory, new NorthwestTerritoryModel() },
                { worldMap.Ontario, new OntarioModel() },
                { worldMap.Quebec, new QuebecModel() },
                { worldMap.WesternUnitedStates, new WesternUnitedStatesModel() },
                { worldMap.Argentina, new ArgentinaModel() },
                { worldMap.Brazil, new BrazilModel() },
                { worldMap.Peru, new PeruModel() },
                { worldMap.Venezuela, new VenezuelaModel() },
                { worldMap.GreatBritain, new GreatBritainModel() },
                { worldMap.Iceland, new IcelandModel() },
                { worldMap.NorthernEurope, new NorthernEuropeModel() },
                { worldMap.Scandinavia, new ScandinaviaModel() },
                { worldMap.SouthernEurope, new SouthernEuropeModel() },
                { worldMap.Ukraine, new UkraineModel() },
                { worldMap.WesternEurope, new WesternEuropeModel() },
                { worldMap.Congo, new CongoModel() },
                { worldMap.EastAfrica, new EastAfricaModel() },
                { worldMap.Egypt, new EgyptModel() },
                { worldMap.Madagascar, new MadagascarModel() },
                { worldMap.NorthAfrica, new NorthAfricaModel() },
                { worldMap.SouthAfrica, new SouthAfricaModel() },
                { worldMap.Afghanistan, new AfghanistanModel() },
                { worldMap.China, new ChinaModel() },
                { worldMap.India, new IndiaModel() },
                { worldMap.Irkutsk, new IrkutskModel() },
                { worldMap.Japan, new JapanModel() },
                { worldMap.Kamchatka, new KamchatkaModel() },
                { worldMap.MiddleEast, new MiddleEastModel() },
                { worldMap.Mongolia, new MongoliaModel() },
                { worldMap.Siam, new SiamModel() },
                { worldMap.Siberia, new SiberiaModel() },
                { worldMap.Ural, new UralModel() },
                { worldMap.Yakutsk, new YakutskModel() },
                { worldMap.EasternAustralia, new EasternAustraliaModel() },
                { worldMap.Indonesia, new IndonesiaModel() },
                { worldMap.NewGuinea, new NewGuineaModel() },
                { worldMap.WesternAustralia, new WesternAustraliaModel() },
            };
        }

        public ITerritoryModel Create(ITerritory territory)
        {
            return _layoutInformation[territory];
        }
    }
}