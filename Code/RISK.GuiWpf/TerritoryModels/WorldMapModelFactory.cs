using System.Collections.Generic;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public interface IWorldMapModelFactory
    {
        IEnumerable<ITerritoryModel> Create(IWorldMap worldMap);
    }

    public class WorldMapModelFactory : IWorldMapModelFactory
    {
        public IEnumerable<ITerritoryModel> Create(IWorldMap worldMap)
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
        }
    }
}