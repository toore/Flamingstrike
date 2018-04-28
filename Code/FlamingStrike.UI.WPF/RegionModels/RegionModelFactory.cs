using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public interface IRegionModelFactory
    {
        IEnumerable<IRegionModel> Create();
    }

    public class RegionModelFactory : IRegionModelFactory
    {
        public IEnumerable<IRegionModel> Create()
        {
            yield return new AlaskaModel(Region.Alaska);
            yield return new AlbertaModel(Region.Alberta);
            yield return new CentralAmericaModel(Region.CentralAmerica);
            yield return new EasternUnitedStatesModel(Region.EasternUnitedStates);
            yield return new GreenlandModel(Region.Greenland);
            yield return new NorthwestTerritoryModel(Region.NorthwestTerritory);
            yield return new OntarioModel(Region.Ontario);
            yield return new QuebecModel(Region.Quebec);
            yield return new WesternUnitedStatesModel(Region.WesternUnitedStates);
            yield return new ArgentinaModel(Region.Argentina);
            yield return new BrazilModel(Region.Brazil);
            yield return new PeruModel(Region.Peru);
            yield return new VenezuelaModel(Region.Venezuela);
            yield return new GreatBritainModel(Region.GreatBritain);
            yield return new IcelandModel(Region.Iceland);
            yield return new NorthernEuropeModel(Region.NorthernEurope);
            yield return new ScandinaviaModel(Region.Scandinavia);
            yield return new SouthernEuropeModel(Region.SouthernEurope);
            yield return new UkraineModel(Region.Ukraine);
            yield return new WesternEuropeModel(Region.WesternEurope);
            yield return new CongoModel(Region.Congo);
            yield return new EastAfricaModel(Region.EastAfrica);
            yield return new EgyptModel(Region.Egypt);
            yield return new MadagascarModel(Region.Madagascar);
            yield return new NorthAfricaModel(Region.NorthAfrica);
            yield return new SouthAfricaModel(Region.SouthAfrica);
            yield return new AfghanistanModel(Region.Afghanistan);
            yield return new ChinaModel(Region.China);
            yield return new IndiaModel(Region.India);
            yield return new IrkutskModel(Region.Irkutsk);
            yield return new JapanModel(Region.Japan);
            yield return new KamchatkaModel(Region.Kamchatka);
            yield return new MiddleEastModel(Region.MiddleEast);
            yield return new MongoliaModel(Region.Mongolia);
            yield return new SiamModel(Region.Siam);
            yield return new SiberiaModel(Region.Siberia);
            yield return new UralModel(Region.Ural);
            yield return new YakutskModel(Region.Yakutsk);
            yield return new EasternAustraliaModel(Region.EasternAustralia);
            yield return new IndonesiaModel(Region.Indonesia);
            yield return new NewGuineaModel(Region.NewGuinea);
            yield return new WesternAustraliaModel(Region.WesternAustralia);
        }
    }
}