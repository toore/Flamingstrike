using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface IWorldMap
    {
        IEnumerable<ITerritoryId> GetAll();

        ITerritoryId Alaska { get; }
        ITerritoryId Alberta { get; }
        ITerritoryId CentralAmerica { get; }
        ITerritoryId EasternUnitedStates { get; }
        ITerritoryId Greenland { get; }
        ITerritoryId NorthwestTerritoryId { get; }
        ITerritoryId Ontario { get; }
        ITerritoryId Quebec { get; }
        ITerritoryId WesternUnitedStates { get; }
        ITerritoryId Argentina { get; }
        ITerritoryId Brazil { get; }
        ITerritoryId Peru { get; }
        ITerritoryId Venezuela { get; }
        ITerritoryId GreatBritain { get; }
        ITerritoryId Iceland { get; }
        ITerritoryId NorthernEurope { get; }
        ITerritoryId Scandinavia { get; }
        ITerritoryId SouthernEurope { get; }
        ITerritoryId Ukraine { get; }
        ITerritoryId WesternEurope { get; }
        ITerritoryId Congo { get; }
        ITerritoryId EastAfrica { get; }
        ITerritoryId Egypt { get; }
        ITerritoryId Madagascar { get; }
        ITerritoryId NorthAfrica { get; }
        ITerritoryId SouthAfrica { get; }
        ITerritoryId Afghanistan { get; }
        ITerritoryId China { get; }
        ITerritoryId India { get; }
        ITerritoryId Irkutsk { get; }
        ITerritoryId Japan { get; }
        ITerritoryId Kamchatka { get; }
        ITerritoryId MiddleEast { get; }
        ITerritoryId Mongolia { get; }
        ITerritoryId Siam { get; }
        ITerritoryId Siberia { get; }
        ITerritoryId Ural { get; }
        ITerritoryId Yakutsk { get; }
        ITerritoryId EasternAustralia { get; }
        ITerritoryId Indonesia { get; }
        ITerritoryId NewGuinea { get; }
        ITerritoryId WesternAustralia { get; }
    }

    public class WorldMap : IWorldMap
    {
        public WorldMap()
        {
            var alaska = new TerritoryId("ALASKA", Continent.NorthAmerica);
            var alberta = new TerritoryId("ALBERTA", Continent.NorthAmerica);
            var centralAmerica = new TerritoryId("CENTRAL_AMERICA", Continent.NorthAmerica);
            var easternUnitedStates = new TerritoryId("EASTERN_UNITED_STATES", Continent.NorthAmerica);
            var greenland = new TerritoryId("GREENLAND", Continent.NorthAmerica);
            var northwestTerritory = new TerritoryId("NORTHWEST_TERRITORY", Continent.NorthAmerica);
            var ontario = new TerritoryId("ONTARIO", Continent.NorthAmerica);
            var quebec = new TerritoryId("QUEBEC", Continent.NorthAmerica);
            var westernUnitedStates = new TerritoryId("WESTERN_UNITED_STATES", Continent.NorthAmerica);

            var argentina = new TerritoryId("ARGENTINA", Continent.SouthAmerica);
            var brazil = new TerritoryId("BRAZIL", Continent.SouthAmerica);
            var peru = new TerritoryId("PERU", Continent.SouthAmerica);
            var venezuela = new TerritoryId("VENEZUELA", Continent.SouthAmerica);

            var greatBritain = new TerritoryId("GREAT_BRITAIN", Continent.Europe);
            var iceland = new TerritoryId("ICELAND", Continent.Europe);
            var northernEurope = new TerritoryId("NORTHERN_EUROPE", Continent.Europe);
            var scandinavia = new TerritoryId("SCANDINAVIA", Continent.Europe);
            var southernEurope = new TerritoryId("SOUTHERN_EUROPE", Continent.Europe);
            var ukraine = new TerritoryId("UKRAINE", Continent.Europe);
            var westernEurope = new TerritoryId("WESTERN_EUROPE", Continent.Europe);

            var congo = new TerritoryId("CONGO", Continent.Africa);
            var eastAfrica = new TerritoryId("EAST_AFRICA", Continent.Africa);
            var egypt = new TerritoryId("EGYPT", Continent.Africa);
            var madagascar = new TerritoryId("MADAGASCAR", Continent.Africa);
            var northAfrica = new TerritoryId("NORTH_AFRICA", Continent.Africa);
            var southAfrica = new TerritoryId("SOUTH_AFRICA", Continent.Africa);

            var afghanistan = new TerritoryId("AFGHANISTAN", Continent.Asia);
            var china = new TerritoryId("CHINA", Continent.Asia);
            var india = new TerritoryId("INDIA", Continent.Asia);
            var irkutsk = new TerritoryId("IRKUTSK", Continent.Asia);
            var japan = new TerritoryId("JAPAN", Continent.Asia);
            var kamchatka = new TerritoryId("KAMCHATKA", Continent.Asia);
            var middleEast = new TerritoryId("MIDDLE_EAST", Continent.Asia);
            var mongolia = new TerritoryId("MONGOLIA", Continent.Asia);
            var siam = new TerritoryId("SIAM", Continent.Asia);
            var siberia = new TerritoryId("SIBERIA", Continent.Asia);
            var ural = new TerritoryId("URAL", Continent.Asia);
            var yakutsk = new TerritoryId("YAKUTSK", Continent.Asia);

            var easternAustralia = new TerritoryId("EASTERN_AUSTRALIA", Continent.Australia);
            var indonesia = new TerritoryId("INDONESIA", Continent.Australia);
            var newGuinea = new TerritoryId("NEW_GUINEA", Continent.Australia);
            var westernAustralia = new TerritoryId("WESTERN_AUSTRALIA", Continent.Australia);

            alaska.AddBorders(alberta, northwestTerritory, kamchatka);
            alberta.AddBorders(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddBorders(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddBorders(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddBorders(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddBorders(alaska, alberta, greenland, ontario);
            ontario.AddBorders(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddBorders(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddBorders(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddBorders(brazil, peru);
            brazil.AddBorders(argentina, peru, venezuela, northAfrica);
            peru.AddBorders(argentina, brazil, venezuela);
            venezuela.AddBorders(brazil, peru, centralAmerica);

            greatBritain.AddBorders(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddBorders(greatBritain, scandinavia, greenland);
            northernEurope.AddBorders(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddBorders(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddBorders(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddBorders(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddBorders(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddBorders(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddBorders(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddBorders(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddBorders(eastAfrica, southAfrica);
            northAfrica.AddBorders(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddBorders(congo, eastAfrica, madagascar);

            afghanistan.AddBorders(china, india, middleEast, ural, ukraine);
            china.AddBorders(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddBorders(afghanistan, china, middleEast, siam);
            irkutsk.AddBorders(kamchatka, mongolia, siberia, yakutsk);
            japan.AddBorders(kamchatka, mongolia);
            kamchatka.AddBorders(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddBorders(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddBorders(china, irkutsk, japan, kamchatka, siberia);
            siam.AddBorders(china, india, indonesia);
            siberia.AddBorders(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddBorders(afghanistan, china, siberia, ukraine);
            yakutsk.AddBorders(irkutsk, kamchatka, siberia);

            easternAustralia.AddBorders(newGuinea, westernAustralia);
            indonesia.AddBorders(newGuinea, westernAustralia, siam);
            newGuinea.AddBorders(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddBorders(easternAustralia, indonesia, newGuinea);

            Alaska = alaska;
            Alberta = alberta;
            CentralAmerica = centralAmerica;
            EasternUnitedStates = easternUnitedStates;
            Greenland = greenland;
            NorthwestTerritoryId = northwestTerritory;
            Ontario = ontario;
            Quebec = quebec;
            WesternUnitedStates = westernUnitedStates;

            Argentina = argentina;
            Brazil = brazil;
            Peru = peru;
            Venezuela = venezuela;

            GreatBritain = greatBritain;
            Iceland = iceland;
            NorthernEurope = northernEurope;
            Scandinavia = scandinavia;
            SouthernEurope = southernEurope;
            Ukraine = ukraine;
            WesternEurope = westernEurope;

            Congo = congo;
            EastAfrica = eastAfrica;
            Egypt = egypt;
            Madagascar = madagascar;
            NorthAfrica = northAfrica;
            SouthAfrica = southAfrica;

            Afghanistan = afghanistan;
            China = china;
            India = india;
            Irkutsk = irkutsk;
            Japan = japan;
            Kamchatka = kamchatka;
            MiddleEast = middleEast;
            Mongolia = mongolia;
            Siam = siam;
            Siberia = siberia;
            Ural = ural;
            Yakutsk = yakutsk;

            EasternAustralia = easternAustralia;
            Indonesia = indonesia;
            NewGuinea = newGuinea;
            WesternAustralia = westernAustralia;
        }

        public IEnumerable<ITerritoryId> GetAll()
        {
            return new[]
            {
                Alaska,
                Alberta,
                CentralAmerica,
                EasternUnitedStates,
                Greenland,
                NorthwestTerritoryId,
                Ontario,
                Quebec,
                WesternUnitedStates,
                Argentina,
                Brazil,
                Peru,
                Venezuela,
                GreatBritain,
                Iceland,
                NorthernEurope,
                Scandinavia,
                SouthernEurope,
                Ukraine,
                WesternEurope,
                Congo,
                EastAfrica,
                Egypt,
                Madagascar,
                NorthAfrica,
                SouthAfrica,
                Afghanistan,
                China,
                India,
                Irkutsk,
                Japan,
                Kamchatka,
                MiddleEast,
                Mongolia,
                Siam,
                Siberia,
                Ural,
                Yakutsk,
                EasternAustralia,
                Indonesia,
                NewGuinea,
                WesternAustralia
            };
        }

        public ITerritoryId Alaska { get; }
        public ITerritoryId Alberta { get; }
        public ITerritoryId CentralAmerica { get; }
        public ITerritoryId EasternUnitedStates { get; }
        public ITerritoryId Greenland { get; }
        public ITerritoryId NorthwestTerritoryId { get; }
        public ITerritoryId Ontario { get; }
        public ITerritoryId Quebec { get; }
        public ITerritoryId WesternUnitedStates { get; }

        public ITerritoryId Argentina { get; }
        public ITerritoryId Brazil { get; }
        public ITerritoryId Peru { get; }
        public ITerritoryId Venezuela { get; }

        public ITerritoryId GreatBritain { get; }
        public ITerritoryId Iceland { get; }
        public ITerritoryId NorthernEurope { get; }
        public ITerritoryId Scandinavia { get; }
        public ITerritoryId SouthernEurope { get; }
        public ITerritoryId Ukraine { get; }
        public ITerritoryId WesternEurope { get; }

        public ITerritoryId Congo { get; }
        public ITerritoryId EastAfrica { get; }
        public ITerritoryId Egypt { get; }
        public ITerritoryId Madagascar { get; }
        public ITerritoryId NorthAfrica { get; }
        public ITerritoryId SouthAfrica { get; }

        public ITerritoryId Afghanistan { get; }
        public ITerritoryId China { get; }
        public ITerritoryId India { get; }
        public ITerritoryId Irkutsk { get; }
        public ITerritoryId Japan { get; }
        public ITerritoryId Kamchatka { get; }
        public ITerritoryId MiddleEast { get; }
        public ITerritoryId Mongolia { get; }
        public ITerritoryId Siam { get; }
        public ITerritoryId Siberia { get; }
        public ITerritoryId Ural { get; }
        public ITerritoryId Yakutsk { get; }

        public ITerritoryId EasternAustralia { get; }
        public ITerritoryId Indonesia { get; }
        public ITerritoryId NewGuinea { get; }
        public ITerritoryId WesternAustralia { get; }
    }
}