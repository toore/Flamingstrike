using System.Collections.Generic;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public interface IWorldMapModelFactory
    {
        IEnumerable<IRegionModel> Create(IRegions regions);
    }

    public class WorldMapModelFactory : IWorldMapModelFactory
    {
        public IEnumerable<IRegionModel> Create(IRegions regions)
        {
            yield return new AlaskaModel(regions.Alaska);
            yield return new AlbertaModel(regions.Alberta);
            yield return new CentralAmericaModel(regions.CentralAmerica);
            yield return new EasternUnitedStatesModel(regions.EasternUnitedStates);
            yield return new GreenlandModel(regions.Greenland);
            yield return new NorthwestRegionModel(regions.NorthwestRegion);
            yield return new OntarioModel(regions.Ontario);
            yield return new QuebecModel(regions.Quebec);
            yield return new WesternUnitedStatesModel(regions.WesternUnitedStates);
            yield return new ArgentinaModel(regions.Argentina);
            yield return new BrazilModel(regions.Brazil);
            yield return new PeruModel(regions.Peru);
            yield return new VenezuelaModel(regions.Venezuela);
            yield return new GreatBritainModel(regions.GreatBritain);
            yield return new IcelandModel(regions.Iceland);
            yield return new NorthernEuropeModel(regions.NorthernEurope);
            yield return new ScandinaviaModel(regions.Scandinavia);
            yield return new SouthernEuropeModel(regions.SouthernEurope);
            yield return new UkraineModel(regions.Ukraine);
            yield return new WesternEuropeModel(regions.WesternEurope);
            yield return new CongoModel(regions.Congo);
            yield return new EastAfricaModel(regions.EastAfrica);
            yield return new EgyptModel(regions.Egypt);
            yield return new MadagascarModel(regions.Madagascar);
            yield return new NorthAfricaModel(regions.NorthAfrica);
            yield return new SouthAfricaModel(regions.SouthAfrica);
            yield return new AfghanistanModel(regions.Afghanistan);
            yield return new ChinaModel(regions.China);
            yield return new IndiaModel(regions.India);
            yield return new IrkutskModel(regions.Irkutsk);
            yield return new JapanModel(regions.Japan);
            yield return new KamchatkaModel(regions.Kamchatka);
            yield return new MiddleEastModel(regions.MiddleEast);
            yield return new MongoliaModel(regions.Mongolia);
            yield return new SiamModel(regions.Siam);
            yield return new SiberiaModel(regions.Siberia);
            yield return new UralModel(regions.Ural);
            yield return new YakutskModel(regions.Yakutsk);
            yield return new EasternAustraliaModel(regions.EasternAustralia);
            yield return new IndonesiaModel(regions.Indonesia);
            yield return new NewGuineaModel(regions.NewGuinea);
            yield return new WesternAustraliaModel(regions.WesternAustralia);
        }
    }
}