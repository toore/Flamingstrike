using System.Collections.Generic;
using RISK.Application;

namespace GuiWpf.TerritoryModels
{
    public interface IWorldMapModelFactory
    {
        IEnumerable<ITerritoryModel> Create(WorldMap worldMap);
    }

    public class WorldMapModelFactory : IWorldMapModelFactory
    {
        public IEnumerable<ITerritoryModel> Create(WorldMap worldMap)
        {
            yield return new AlaskaModel(worldMap.Alaska);
            yield return new AlbertaModel(worldMap.Alberta);
            yield return new CentralAmericaModel(worldMap.CentralAmerica);
            yield return new EasternUnitedStatesModel(worldMap.EasternUnitedStates);
            yield return new GreenlandModel(worldMap.Greenland);
            yield return new NorthwestTerritoryModel(worldMap.NorthwestTerritory);
            yield return new OntarioModel(worldMap.Ontario);
            yield return new QuebecModel(worldMap.Quebec);
            yield return new WesternUnitedStatesModel(worldMap.WesternUnitedStates);
            yield return new ArgentinaModel(worldMap.Argentina);
            yield return new BrazilModel(worldMap.Brazil);
            yield return new PeruModel(worldMap.Peru);
            yield return new VenezuelaModel(worldMap.Venezuela);
            yield return new GreatBritainModel(worldMap.GreatBritain);
            yield return new IcelandModel(worldMap.Iceland);
            yield return new NorthernEuropeModel(worldMap.NorthernEurope);
            yield return new ScandinaviaModel(worldMap.Scandinavia);
            yield return new SouthernEuropeModel(worldMap.SouthernEurope);
            yield return new UkraineModel(worldMap.Ukraine);
            yield return new WesternEuropeModel(worldMap.WesternEurope);
            yield return new CongoModel(worldMap.Congo);
            yield return new EastAfricaModel(worldMap.EastAfrica);
            yield return new EgyptModel(worldMap.Egypt);
            yield return new MadagascarModel(worldMap.Madagascar);
            yield return new NorthAfricaModel(worldMap.NorthAfrica);
            yield return new SouthAfricaModel(worldMap.SouthAfrica);
            yield return new AfghanistanModel(worldMap.Afghanistan);
            yield return new ChinaModel(worldMap.China);
            yield return new IndiaModel(worldMap.India);
            yield return new IrkutskModel(worldMap.Irkutsk);
            yield return new JapanModel(worldMap.Japan);
            yield return new KamchatkaModel(worldMap.Kamchatka);
            yield return new MiddleEastModel(worldMap.MiddleEast);
            yield return new MongoliaModel(worldMap.Mongolia);
            yield return new SiamModel(worldMap.Siam);
            yield return new SiberiaModel(worldMap.Siberia);
            yield return new UralModel(worldMap.Ural);
            yield return new YakutskModel(worldMap.Yakutsk);
            yield return new EasternAustraliaModel(worldMap.EasternAustralia);
            yield return new IndonesiaModel(worldMap.Indonesia);
            yield return new NewGuineaModel(worldMap.NewGuinea);
            yield return new WesternAustraliaModel(worldMap.WesternAustralia);


            //var territoryModels = new Dictionary<ITerritory, ITerritoryModel>
            //{
            //    { worldMap.Alaska, new AlaskaModel() },
            //    { worldMap.Alberta, new AlbertaModel() },
            //    { worldMap.CentralAmerica, new CentralAmericaModel() },
            //    { worldMap.EasternUnitedStates, new EasternUnitedStatesModel() },
            //    { worldMap.Greenland, new GreenlandModel() },
            //    { worldMap.NorthwestTerritory, new NorthwestTerritoryModel() },
            //    { worldMap.Ontario, new OntarioModel() },
            //    { worldMap.Quebec, new QuebecModel() },
            //    { worldMap.WesternUnitedStates, new WesternUnitedStatesModel() },
            //    { worldMap.Argentina, new ArgentinaModel() },
            //    { worldMap.Brazil, new BrazilModel() },
            //    { worldMap.Peru, new PeruModel() },
            //    { worldMap.Venezuela, new VenezuelaModel() },
            //    { worldMap.GreatBritain, new GreatBritainModel() },
            //    { worldMap.Iceland, new IcelandModel() },
            //    { worldMap.NorthernEurope, new NorthernEuropeModel() },
            //    { worldMap.Scandinavia, new ScandinaviaModel() },
            //    { worldMap.SouthernEurope, new SouthernEuropeModel() },
            //    { worldMap.Ukraine, new UkraineModel() },
            //    { worldMap.WesternEurope, new WesternEuropeModel() },
            //    { worldMap.Congo, new CongoModel() },
            //    { worldMap.EastAfrica, new EastAfricaModel() },
            //    { worldMap.Egypt, new EgyptModel() },
            //    { worldMap.Madagascar, new MadagascarModel() },
            //    { worldMap.NorthAfrica, new NorthAfricaModel() },
            //    { worldMap.SouthAfrica, new SouthAfricaModel() },
            //    { worldMap.Afghanistan, new AfghanistanModel() },
            //    { worldMap.China, new ChinaModel() },
            //    { worldMap.India, new IndiaModel() },
            //    { worldMap.Irkutsk, new IrkutskModel() },
            //    { worldMap.Japan, new JapanModel() },
            //    { worldMap.Kamchatka, new KamchatkaModel() },
            //    { worldMap.MiddleEast, new MiddleEastModel() },
            //    { worldMap.Mongolia, new MongoliaModel() },
            //    { worldMap.Siam, new SiamModel() },
            //    { worldMap.Siberia, new SiberiaModel() },
            //    { worldMap.Ural, new UralModel() },
            //    { worldMap.Yakutsk, new YakutskModel() },
            //    { worldMap.EasternAustralia, new EasternAustraliaModel() },
            //    { worldMap.Indonesia, new IndonesiaModel() },
            //    { worldMap.NewGuinea, new NewGuineaModel() },
            //    { worldMap.WesternAustralia, new WesternAustraliaModel() },
            //};

            //return 
        }
    }
}