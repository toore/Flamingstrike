using System.Collections.Generic;
using RISK.GameEngine;

namespace RISK.UI.WPF.RegionModels
{
    public interface IRegionModelFactory
    {
        IEnumerable<IRegionModel> Create();
    }

    public class RegionModelFactory : IRegionModelFactory
    {
        private readonly IRegions _regions;

        public RegionModelFactory(IRegions regions)
        {
            _regions = regions;
        }

        public IEnumerable<IRegionModel> Create()
        {
            yield return new AlaskaModel(_regions.Alaska);
            yield return new AlbertaModel(_regions.Alberta);
            yield return new CentralAmericaModel(_regions.CentralAmerica);
            yield return new EasternUnitedStatesModel(_regions.EasternUnitedStates);
            yield return new GreenlandModel(_regions.Greenland);
            yield return new NorthwestRegionModel(_regions.NorthwestRegion);
            yield return new OntarioModel(_regions.Ontario);
            yield return new QuebecModel(_regions.Quebec);
            yield return new WesternUnitedStatesModel(_regions.WesternUnitedStates);
            yield return new ArgentinaModel(_regions.Argentina);
            yield return new BrazilModel(_regions.Brazil);
            yield return new PeruModel(_regions.Peru);
            yield return new VenezuelaModel(_regions.Venezuela);
            yield return new GreatBritainModel(_regions.GreatBritain);
            yield return new IcelandModel(_regions.Iceland);
            yield return new NorthernEuropeModel(_regions.NorthernEurope);
            yield return new ScandinaviaModel(_regions.Scandinavia);
            yield return new SouthernEuropeModel(_regions.SouthernEurope);
            yield return new UkraineModel(_regions.Ukraine);
            yield return new WesternEuropeModel(_regions.WesternEurope);
            yield return new CongoModel(_regions.Congo);
            yield return new EastAfricaModel(_regions.EastAfrica);
            yield return new EgyptModel(_regions.Egypt);
            yield return new MadagascarModel(_regions.Madagascar);
            yield return new NorthAfricaModel(_regions.NorthAfrica);
            yield return new SouthAfricaModel(_regions.SouthAfrica);
            yield return new AfghanistanModel(_regions.Afghanistan);
            yield return new ChinaModel(_regions.China);
            yield return new IndiaModel(_regions.India);
            yield return new IrkutskModel(_regions.Irkutsk);
            yield return new JapanModel(_regions.Japan);
            yield return new KamchatkaModel(_regions.Kamchatka);
            yield return new MiddleEastModel(_regions.MiddleEast);
            yield return new MongoliaModel(_regions.Mongolia);
            yield return new SiamModel(_regions.Siam);
            yield return new SiberiaModel(_regions.Siberia);
            yield return new UralModel(_regions.Ural);
            yield return new YakutskModel(_regions.Yakutsk);
            yield return new EasternAustraliaModel(_regions.EasternAustralia);
            yield return new IndonesiaModel(_regions.Indonesia);
            yield return new NewGuineaModel(_regions.NewGuinea);
            yield return new WesternAustraliaModel(_regions.WesternAustralia);
        }
    }
}