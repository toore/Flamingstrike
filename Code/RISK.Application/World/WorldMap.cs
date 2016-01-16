using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface IWorldMap
    {
        IEnumerable<ITerritoryGeography> GetAll();

        ITerritoryGeography Alaska { get; }
        ITerritoryGeography Alberta { get; }
        ITerritoryGeography CentralAmerica { get; }
        ITerritoryGeography EasternUnitedStates { get; }
        ITerritoryGeography Greenland { get; }
        ITerritoryGeography NorthwestTerritoryGeography { get; }
        ITerritoryGeography Ontario { get; }
        ITerritoryGeography Quebec { get; }
        ITerritoryGeography WesternUnitedStates { get; }
        ITerritoryGeography Argentina { get; }
        ITerritoryGeography Brazil { get; }
        ITerritoryGeography Peru { get; }
        ITerritoryGeography Venezuela { get; }
        ITerritoryGeography GreatBritain { get; }
        ITerritoryGeography Iceland { get; }
        ITerritoryGeography NorthernEurope { get; }
        ITerritoryGeography Scandinavia { get; }
        ITerritoryGeography SouthernEurope { get; }
        ITerritoryGeography Ukraine { get; }
        ITerritoryGeography WesternEurope { get; }
        ITerritoryGeography Congo { get; }
        ITerritoryGeography EastAfrica { get; }
        ITerritoryGeography Egypt { get; }
        ITerritoryGeography Madagascar { get; }
        ITerritoryGeography NorthAfrica { get; }
        ITerritoryGeography SouthAfrica { get; }
        ITerritoryGeography Afghanistan { get; }
        ITerritoryGeography China { get; }
        ITerritoryGeography India { get; }
        ITerritoryGeography Irkutsk { get; }
        ITerritoryGeography Japan { get; }
        ITerritoryGeography Kamchatka { get; }
        ITerritoryGeography MiddleEast { get; }
        ITerritoryGeography Mongolia { get; }
        ITerritoryGeography Siam { get; }
        ITerritoryGeography Siberia { get; }
        ITerritoryGeography Ural { get; }
        ITerritoryGeography Yakutsk { get; }
        ITerritoryGeography EasternAustralia { get; }
        ITerritoryGeography Indonesia { get; }
        ITerritoryGeography NewGuinea { get; }
        ITerritoryGeography WesternAustralia { get; }
    }

    public class WorldMap : IWorldMap
    {
        public WorldMap()
        {
            var alaska = new TerritoryGeography("ALASKA", Continent.NorthAmerica);
            var alberta = new TerritoryGeography("ALBERTA", Continent.NorthAmerica);
            var centralAmerica = new TerritoryGeography("CENTRAL_AMERICA", Continent.NorthAmerica);
            var easternUnitedStates = new TerritoryGeography("EASTERN_UNITED_STATES", Continent.NorthAmerica);
            var greenland = new TerritoryGeography("GREENLAND", Continent.NorthAmerica);
            var northwestTerritory = new TerritoryGeography("NORTHWEST_TERRITORY", Continent.NorthAmerica);
            var ontario = new TerritoryGeography("ONTARIO", Continent.NorthAmerica);
            var quebec = new TerritoryGeography("QUEBEC", Continent.NorthAmerica);
            var westernUnitedStates = new TerritoryGeography("WESTERN_UNITED_STATES", Continent.NorthAmerica);

            var argentina = new TerritoryGeography("ARGENTINA", Continent.SouthAmerica);
            var brazil = new TerritoryGeography("BRAZIL", Continent.SouthAmerica);
            var peru = new TerritoryGeography("PERU", Continent.SouthAmerica);
            var venezuela = new TerritoryGeography("VENEZUELA", Continent.SouthAmerica);

            var greatBritain = new TerritoryGeography("GREAT_BRITAIN", Continent.Europe);
            var iceland = new TerritoryGeography("ICELAND", Continent.Europe);
            var northernEurope = new TerritoryGeography("NORTHERN_EUROPE", Continent.Europe);
            var scandinavia = new TerritoryGeography("SCANDINAVIA", Continent.Europe);
            var southernEurope = new TerritoryGeography("SOUTHERN_EUROPE", Continent.Europe);
            var ukraine = new TerritoryGeography("UKRAINE", Continent.Europe);
            var westernEurope = new TerritoryGeography("WESTERN_EUROPE", Continent.Europe);

            var congo = new TerritoryGeography("CONGO", Continent.Africa);
            var eastAfrica = new TerritoryGeography("EAST_AFRICA", Continent.Africa);
            var egypt = new TerritoryGeography("EGYPT", Continent.Africa);
            var madagascar = new TerritoryGeography("MADAGASCAR", Continent.Africa);
            var northAfrica = new TerritoryGeography("NORTH_AFRICA", Continent.Africa);
            var southAfrica = new TerritoryGeography("SOUTH_AFRICA", Continent.Africa);

            var afghanistan = new TerritoryGeography("AFGHANISTAN", Continent.Asia);
            var china = new TerritoryGeography("CHINA", Continent.Asia);
            var india = new TerritoryGeography("INDIA", Continent.Asia);
            var irkutsk = new TerritoryGeography("IRKUTSK", Continent.Asia);
            var japan = new TerritoryGeography("JAPAN", Continent.Asia);
            var kamchatka = new TerritoryGeography("KAMCHATKA", Continent.Asia);
            var middleEast = new TerritoryGeography("MIDDLE_EAST", Continent.Asia);
            var mongolia = new TerritoryGeography("MONGOLIA", Continent.Asia);
            var siam = new TerritoryGeography("SIAM", Continent.Asia);
            var siberia = new TerritoryGeography("SIBERIA", Continent.Asia);
            var ural = new TerritoryGeography("URAL", Continent.Asia);
            var yakutsk = new TerritoryGeography("YAKUTSK", Continent.Asia);

            var easternAustralia = new TerritoryGeography("EASTERN_AUSTRALIA", Continent.Australia);
            var indonesia = new TerritoryGeography("INDONESIA", Continent.Australia);
            var newGuinea = new TerritoryGeography("NEW_GUINEA", Continent.Australia);
            var westernAustralia = new TerritoryGeography("WESTERN_AUSTRALIA", Continent.Australia);

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
            NorthwestTerritoryGeography = northwestTerritory;
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

        public IEnumerable<ITerritoryGeography> GetAll()
        {
            return new[]
            {
                Alaska,
                Alberta,
                CentralAmerica,
                EasternUnitedStates,
                Greenland,
                NorthwestTerritoryGeography,
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

        public ITerritoryGeography Alaska { get; }
        public ITerritoryGeography Alberta { get; }
        public ITerritoryGeography CentralAmerica { get; }
        public ITerritoryGeography EasternUnitedStates { get; }
        public ITerritoryGeography Greenland { get; }
        public ITerritoryGeography NorthwestTerritoryGeography { get; }
        public ITerritoryGeography Ontario { get; }
        public ITerritoryGeography Quebec { get; }
        public ITerritoryGeography WesternUnitedStates { get; }

        public ITerritoryGeography Argentina { get; }
        public ITerritoryGeography Brazil { get; }
        public ITerritoryGeography Peru { get; }
        public ITerritoryGeography Venezuela { get; }

        public ITerritoryGeography GreatBritain { get; }
        public ITerritoryGeography Iceland { get; }
        public ITerritoryGeography NorthernEurope { get; }
        public ITerritoryGeography Scandinavia { get; }
        public ITerritoryGeography SouthernEurope { get; }
        public ITerritoryGeography Ukraine { get; }
        public ITerritoryGeography WesternEurope { get; }

        public ITerritoryGeography Congo { get; }
        public ITerritoryGeography EastAfrica { get; }
        public ITerritoryGeography Egypt { get; }
        public ITerritoryGeography Madagascar { get; }
        public ITerritoryGeography NorthAfrica { get; }
        public ITerritoryGeography SouthAfrica { get; }

        public ITerritoryGeography Afghanistan { get; }
        public ITerritoryGeography China { get; }
        public ITerritoryGeography India { get; }
        public ITerritoryGeography Irkutsk { get; }
        public ITerritoryGeography Japan { get; }
        public ITerritoryGeography Kamchatka { get; }
        public ITerritoryGeography MiddleEast { get; }
        public ITerritoryGeography Mongolia { get; }
        public ITerritoryGeography Siam { get; }
        public ITerritoryGeography Siberia { get; }
        public ITerritoryGeography Ural { get; }
        public ITerritoryGeography Yakutsk { get; }

        public ITerritoryGeography EasternAustralia { get; }
        public ITerritoryGeography Indonesia { get; }
        public ITerritoryGeography NewGuinea { get; }
        public ITerritoryGeography WesternAustralia { get; }
    }
}