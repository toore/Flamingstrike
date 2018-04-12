namespace FlamingStrike.GameEngine
{
    public class WorldMapFactory
    {
        public IWorldMap Create()
        {
            var worldMap = new WorldMap();

            worldMap.AddBorder(Region.Alaska, Region.Alberta);
            worldMap.AddBorder(Region.Alaska, Region.NorthwestTerritory);
            worldMap.AddBorder(Region.Alaska, Region.Kamchatka);
            worldMap.AddBorder(Region.Alberta, Region.NorthwestTerritory);
            worldMap.AddBorder(Region.Alberta, Region.Ontario);
            worldMap.AddBorder(Region.Alberta, Region.WesternUnitedStates);
            worldMap.AddBorder(Region.CentralAmerica, Region.EasternUnitedStates);
            worldMap.AddBorder(Region.CentralAmerica, Region.WesternUnitedStates);
            worldMap.AddBorder(Region.CentralAmerica, Region.Venezuela);
            worldMap.AddBorder(Region.EasternUnitedStates, Region.Ontario);
            worldMap.AddBorder(Region.EasternUnitedStates, Region.Quebec);
            worldMap.AddBorder(Region.EasternUnitedStates, Region.WesternUnitedStates);
            worldMap.AddBorder(Region.Greenland, Region.NorthwestTerritory);
            worldMap.AddBorder(Region.Greenland, Region.Ontario);
            worldMap.AddBorder(Region.Greenland, Region.Quebec);
            worldMap.AddBorder(Region.Greenland, Region.Iceland);
            worldMap.AddBorder(Region.NorthwestTerritory, Region.Ontario);
            worldMap.AddBorder(Region.Ontario, Region.Quebec);
            worldMap.AddBorder(Region.Ontario, Region.WesternUnitedStates);

            worldMap.AddBorder(Region.Argentina, Region.Brazil);
            worldMap.AddBorder(Region.Argentina, Region.Peru);
            worldMap.AddBorder(Region.Brazil, Region.Peru);
            worldMap.AddBorder(Region.Brazil, Region.Venezuela);
            worldMap.AddBorder(Region.Brazil, Region.NorthAfrica);
            worldMap.AddBorder(Region.Peru, Region.Venezuela);

            worldMap.AddBorder(Region.GreatBritain, Region.Iceland);
            worldMap.AddBorder(Region.GreatBritain, Region.NorthernEurope);
            worldMap.AddBorder(Region.GreatBritain, Region.Scandinavia);
            worldMap.AddBorder(Region.GreatBritain, Region.WesternEurope);
            worldMap.AddBorder(Region.Iceland, Region.Scandinavia);
            worldMap.AddBorder(Region.NorthernEurope, Region.Scandinavia);
            worldMap.AddBorder(Region.NorthernEurope, Region.SouthernEurope);
            worldMap.AddBorder(Region.NorthernEurope, Region.Ukraine);
            worldMap.AddBorder(Region.NorthernEurope, Region.WesternEurope);
            worldMap.AddBorder(Region.Scandinavia, Region.Ukraine);
            worldMap.AddBorder(Region.SouthernEurope, Region.Ukraine);
            worldMap.AddBorder(Region.SouthernEurope, Region.WesternEurope);
            worldMap.AddBorder(Region.SouthernEurope, Region.Egypt);
            worldMap.AddBorder(Region.SouthernEurope, Region.NorthAfrica);
            worldMap.AddBorder(Region.SouthernEurope, Region.MiddleEast);
            worldMap.AddBorder(Region.Ukraine, Region.Afghanistan);
            worldMap.AddBorder(Region.Ukraine, Region.MiddleEast);
            worldMap.AddBorder(Region.Ukraine, Region.Ural);
            worldMap.AddBorder(Region.WesternEurope, Region.NorthAfrica);

            worldMap.AddBorder(Region.Congo, Region.EastAfrica);
            worldMap.AddBorder(Region.Congo, Region.NorthAfrica);
            worldMap.AddBorder(Region.Congo, Region.SouthAfrica);
            worldMap.AddBorder(Region.EastAfrica, Region.Egypt);
            worldMap.AddBorder(Region.EastAfrica, Region.Madagascar);
            worldMap.AddBorder(Region.EastAfrica, Region.NorthAfrica);
            worldMap.AddBorder(Region.EastAfrica, Region.SouthAfrica);
            worldMap.AddBorder(Region.EastAfrica, Region.MiddleEast);
            worldMap.AddBorder(Region.Egypt, Region.NorthAfrica);
            worldMap.AddBorder(Region.Egypt, Region.MiddleEast);
            worldMap.AddBorder(Region.Madagascar, Region.SouthAfrica);

            worldMap.AddBorder(Region.Afghanistan, Region.China);
            worldMap.AddBorder(Region.Afghanistan, Region.India);
            worldMap.AddBorder(Region.Afghanistan, Region.MiddleEast);
            worldMap.AddBorder(Region.Afghanistan, Region.Ural);
            worldMap.AddBorder(Region.China, Region.India);
            worldMap.AddBorder(Region.China, Region.Mongolia);
            worldMap.AddBorder(Region.China, Region.Siam);
            worldMap.AddBorder(Region.China, Region.Siberia);
            worldMap.AddBorder(Region.China, Region.Ural);
            worldMap.AddBorder(Region.India, Region.MiddleEast);
            worldMap.AddBorder(Region.India, Region.Siam);
            worldMap.AddBorder(Region.Irkutsk, Region.Kamchatka);
            worldMap.AddBorder(Region.Irkutsk, Region.Mongolia);
            worldMap.AddBorder(Region.Irkutsk, Region.Siberia);
            worldMap.AddBorder(Region.Irkutsk, Region.Yakutsk);
            worldMap.AddBorder(Region.Japan, Region.Kamchatka);
            worldMap.AddBorder(Region.Japan, Region.Mongolia);
            worldMap.AddBorder(Region.Kamchatka, Region.Mongolia);
            worldMap.AddBorder(Region.Kamchatka, Region.Yakutsk);
            worldMap.AddBorder(Region.Mongolia, Region.Siberia);
            worldMap.AddBorder(Region.Siam, Region.Indonesia);
            worldMap.AddBorder(Region.Siberia, Region.Ural);
            worldMap.AddBorder(Region.Siberia, Region.Yakutsk);

            worldMap.AddBorder(Region.EasternAustralia, Region.NewGuinea);
            worldMap.AddBorder(Region.EasternAustralia, Region.WesternAustralia);
            worldMap.AddBorder(Region.Indonesia, Region.NewGuinea);
            worldMap.AddBorder(Region.Indonesia, Region.WesternAustralia);
            worldMap.AddBorder(Region.NewGuinea, Region.WesternAustralia);

            return worldMap;
        }
    }

    //public class Regions
    //{
    //    public Regions()
    //    {
    //        var alaska = new Region(Continent.NorthAmerica);
    //        var alberta = new Region(Continent.NorthAmerica);
    //        var centralAmerica = new Region(Continent.NorthAmerica);
    //        var easternUnitedStates = new Region(Continent.NorthAmerica);
    //        var greenland = new Region(Continent.NorthAmerica);
    //        var northwestTerritory = new Region(Continent.NorthAmerica);
    //        var ontario = new Region(Continent.NorthAmerica);
    //        var quebec = new Region(Continent.NorthAmerica);
    //        var westernUnitedStates = new Region(Continent.NorthAmerica);

    //        var argentina = new Region(Continent.SouthAmerica);
    //        var brazil = new Region(Continent.SouthAmerica);
    //        var peru = new Region(Continent.SouthAmerica);
    //        var venezuela = new Region(Continent.SouthAmerica);

    //        var greatBritain = new Region(Continent.Europe);
    //        var iceland = new Region(Continent.Europe);
    //        var northernEurope = new Region(Continent.Europe);
    //        var scandinavia = new Region(Continent.Europe);
    //        var southernEurope = new Region(Continent.Europe);
    //        var ukraine = new Region(Continent.Europe);
    //        var westernEurope = new Region(Continent.Europe);

    //        var congo = new Region(Continent.Africa);
    //        var eastAfrica = new Region(Continent.Africa);
    //        var egypt = new Region(Continent.Africa);
    //        var madagascar = new Region(Continent.Africa);
    //        var northAfrica = new Region(Continent.Africa);
    //        var southAfrica = new Region(Continent.Africa);

    //        var afghanistan = new Region(Continent.Asia);
    //        var china = new Region(Continent.Asia);
    //        var india = new Region(Continent.Asia);
    //        var irkutsk = new Region(Continent.Asia);
    //        var japan = new Region(Continent.Asia);
    //        var kamchatka = new Region(Continent.Asia);
    //        var middleEast = new Region(Continent.Asia);
    //        var mongolia = new Region(Continent.Asia);
    //        var siam = new Region(Continent.Asia);
    //        var siberia = new Region(Continent.Asia);
    //        var ural = new Region(Continent.Asia);
    //        var yakutsk = new Region(Continent.Asia);

    //        var easternAustralia = new Region(Continent.Australia);
    //        var indonesia = new Region(Continent.Australia);
    //        var newGuinea = new Region(Continent.Australia);
    //        var westernAustralia = new Region(Continent.Australia);

    //        alaska.AddBordersToRegions(alberta, northwestTerritory, kamchatka);
    //        alberta.AddBordersToRegions(alaska, northwestTerritory, ontario, westernUnitedStates);
    //        centralAmerica.AddBordersToRegions(easternUnitedStates, westernUnitedStates, venezuela);
    //        easternUnitedStates.AddBordersToRegions(centralAmerica, ontario, quebec, westernUnitedStates);
    //        greenland.AddBordersToRegions(northwestTerritory, ontario, quebec, iceland);
    //        northwestTerritory.AddBordersToRegions(alaska, alberta, greenland, ontario);
    //        ontario.AddBordersToRegions(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
    //        quebec.AddBordersToRegions(easternUnitedStates, greenland, ontario);
    //        westernUnitedStates.AddBordersToRegions(alberta, centralAmerica, easternUnitedStates, ontario);

    //        argentina.AddBordersToRegions(brazil, peru);
    //        brazil.AddBordersToRegions(argentina, peru, venezuela, northAfrica);
    //        peru.AddBordersToRegions(argentina, brazil, venezuela);
    //        venezuela.AddBordersToRegions(brazil, peru, centralAmerica);

    //        greatBritain.AddBordersToRegions(iceland, northernEurope, scandinavia, westernEurope);
    //        iceland.AddBordersToRegions(greatBritain, scandinavia, greenland);
    //        northernEurope.AddBordersToRegions(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
    //        scandinavia.AddBordersToRegions(greatBritain, iceland, northernEurope, ukraine);
    //        southernEurope.AddBordersToRegions(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
    //        ukraine.AddBordersToRegions(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
    //        westernEurope.AddBordersToRegions(greatBritain, northernEurope, southernEurope, northAfrica);

    //        congo.AddBordersToRegions(eastAfrica, northAfrica, southAfrica);
    //        eastAfrica.AddBordersToRegions(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
    //        egypt.AddBordersToRegions(eastAfrica, northAfrica, southernEurope, middleEast);
    //        madagascar.AddBordersToRegions(eastAfrica, southAfrica);
    //        northAfrica.AddBordersToRegions(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
    //        southAfrica.AddBordersToRegions(congo, eastAfrica, madagascar);

    //        afghanistan.AddBordersToRegions(china, india, middleEast, ural, ukraine);
    //        china.AddBordersToRegions(afghanistan, india, mongolia, siam, siberia, ural);
    //        india.AddBordersToRegions(afghanistan, china, middleEast, siam);
    //        irkutsk.AddBordersToRegions(kamchatka, mongolia, siberia, yakutsk);
    //        japan.AddBordersToRegions(kamchatka, mongolia);
    //        kamchatka.AddBordersToRegions(irkutsk, japan, mongolia, yakutsk, alaska);
    //        middleEast.AddBordersToRegions(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
    //        mongolia.AddBordersToRegions(china, irkutsk, japan, kamchatka, siberia);
    //        siam.AddBordersToRegions(china, india, indonesia);
    //        siberia.AddBordersToRegions(china, irkutsk, mongolia, ural, yakutsk);
    //        ural.AddBordersToRegions(afghanistan, china, siberia, ukraine);
    //        yakutsk.AddBordersToRegions(irkutsk, kamchatka, siberia);

    //        easternAustralia.AddBordersToRegions(newGuinea, westernAustralia);
    //        indonesia.AddBordersToRegions(newGuinea, westernAustralia, siam);
    //        newGuinea.AddBordersToRegions(easternAustralia, indonesia, westernAustralia);
    //        westernAustralia.AddBordersToRegions(easternAustralia, indonesia, newGuinea);

    //        Alaska = alaska;
    //        Alberta = alberta;
    //        CentralAmerica = centralAmerica;
    //        EasternUnitedStates = easternUnitedStates;
    //        Greenland = greenland;
    //        NorthwestRegion = northwestTerritory;
    //        Ontario = ontario;
    //        Quebec = quebec;
    //        WesternUnitedStates = westernUnitedStates;

    //        Argentina = argentina;
    //        Brazil = brazil;
    //        Peru = peru;
    //        Venezuela = venezuela;

    //        GreatBritain = greatBritain;
    //        Iceland = iceland;
    //        NorthernEurope = northernEurope;
    //        Scandinavia = scandinavia;
    //        SouthernEurope = southernEurope;
    //        Ukraine = ukraine;
    //        WesternEurope = westernEurope;

    //        Congo = congo;
    //        EastAfrica = eastAfrica;
    //        Egypt = egypt;
    //        Madagascar = madagascar;
    //        NorthAfrica = northAfrica;
    //        SouthAfrica = southAfrica;

    //        Afghanistan = afghanistan;
    //        China = china;
    //        India = india;
    //        Irkutsk = irkutsk;
    //        Japan = japan;
    //        Kamchatka = kamchatka;
    //        MiddleEast = middleEast;
    //        Mongolia = mongolia;
    //        Siam = siam;
    //        Siberia = siberia;
    //        Ural = ural;
    //        Yakutsk = yakutsk;

    //        EasternAustralia = easternAustralia;
    //        Indonesia = indonesia;
    //        NewGuinea = newGuinea;
    //        WesternAustralia = westernAustralia;
    //    }

    //    public IEnumerable<Region> GetAll()
    //    {
    //        return new[]
    //        {
    //            Alaska,
    //            Alberta,
    //            CentralAmerica,
    //            EasternUnitedStates,
    //            Greenland,
    //            NorthwestRegion,
    //            Ontario,
    //            Quebec,
    //            WesternUnitedStates,
    //            Argentina,
    //            Brazil,
    //            Peru,
    //            Venezuela,
    //            GreatBritain,
    //            Iceland,
    //            NorthernEurope,
    //            Scandinavia,
    //            SouthernEurope,
    //            Ukraine,
    //            WesternEurope,
    //            Congo,
    //            EastAfrica,
    //            Egypt,
    //            Madagascar,
    //            NorthAfrica,
    //            SouthAfrica,
    //            Afghanistan,
    //            China,
    //            India,
    //            Irkutsk,
    //            Japan,
    //            Kamchatka,
    //            MiddleEast,
    //            Mongolia,
    //            Siam,
    //            Siberia,
    //            Ural,
    //            Yakutsk,
    //            EasternAustralia,
    //            Indonesia,
    //            NewGuinea,
    //            WesternAustralia
    //        };
    //    }

    //    public Region Alaska { get; }
    //    public Region Alberta { get; }
    //    public Region CentralAmerica { get; }
    //    public Region EasternUnitedStates { get; }
    //    public Region Greenland { get; }
    //    public Region NorthwestRegion { get; }
    //    public Region Ontario { get; }
    //    public Region Quebec { get; }
    //    public Region WesternUnitedStates { get; }

    //    public Region Argentina { get; }
    //    public Region Brazil { get; }
    //    public Region Peru { get; }
    //    public Region Venezuela { get; }

    //    public Region GreatBritain { get; }
    //    public Region Iceland { get; }
    //    public Region NorthernEurope { get; }
    //    public Region Scandinavia { get; }
    //    public Region SouthernEurope { get; }
    //    public Region Ukraine { get; }
    //    public Region WesternEurope { get; }

    //    public Region Congo { get; }
    //    public Region EastAfrica { get; }
    //    public Region Egypt { get; }
    //    public Region Madagascar { get; }
    //    public Region NorthAfrica { get; }
    //    public Region SouthAfrica { get; }

    //    public Region Afghanistan { get; }
    //    public Region China { get; }
    //    public Region India { get; }
    //    public Region Irkutsk { get; }
    //    public Region Japan { get; }
    //    public Region Kamchatka { get; }
    //    public Region MiddleEast { get; }
    //    public Region Mongolia { get; }
    //    public Region Siam { get; }
    //    public Region Siberia { get; }
    //    public Region Ural { get; }
    //    public Region Yakutsk { get; }

    //    public Region EasternAustralia { get; }
    //    public Region Indonesia { get; }
    //    public Region NewGuinea { get; }
    //    public Region WesternAustralia { get; }
    //}
}