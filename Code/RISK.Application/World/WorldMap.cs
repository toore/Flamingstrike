using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface IWorldMap
    {
        IEnumerable<ITerritory> GetAll();

        ITerritory Alaska { get; }
        ITerritory Alberta { get; }
        ITerritory CentralAmerica { get; }
        ITerritory EasternUnitedStates { get; }
        ITerritory Greenland { get; }
        ITerritory NorthwestTerritory { get; }
        ITerritory Ontario { get; }
        ITerritory Quebec { get; }
        ITerritory WesternUnitedStates { get; }
        ITerritory Argentina { get; }
        ITerritory Brazil { get; }
        ITerritory Peru { get; }
        ITerritory Venezuela { get; }
        ITerritory GreatBritain { get; }
        ITerritory Iceland { get; }
        ITerritory NorthernEurope { get; }
        ITerritory Scandinavia { get; }
        ITerritory SouthernEurope { get; }
        ITerritory Ukraine { get; }
        ITerritory WesternEurope { get; }
        ITerritory Congo { get; }
        ITerritory EastAfrica { get; }
        ITerritory Egypt { get; }
        ITerritory Madagascar { get; }
        ITerritory NorthAfrica { get; }
        ITerritory SouthAfrica { get; }
        ITerritory Afghanistan { get; }
        ITerritory China { get; }
        ITerritory India { get; }
        ITerritory Irkutsk { get; }
        ITerritory Japan { get; }
        ITerritory Kamchatka { get; }
        ITerritory MiddleEast { get; }
        ITerritory Mongolia { get; }
        ITerritory Siam { get; }
        ITerritory Siberia { get; }
        ITerritory Ural { get; }
        ITerritory Yakutsk { get; }
        ITerritory EasternAustralia { get; }
        ITerritory Indonesia { get; }
        ITerritory NewGuinea { get; }
        ITerritory WesternAustralia { get; }

        //IEnumerable<ITerritory> GetTerritoriesOccupiedByPlayer(IPlayer player);
        //IEnumerable<IPlayer> GetAllPlayersOccupyingTerritories();
    }

    public class WorldMap : IWorldMap
    {
        public WorldMap()
        {
            var alaska = new Territory("ALASKA", Continent.NorthAmerica);
            var alberta = new Territory("ALBERTA", Continent.NorthAmerica);
            var centralAmerica = new Territory("CENTRAL_AMERICA", Continent.NorthAmerica);
            var easternUnitedStates = new Territory("EASTERN_UNITED_STATES", Continent.NorthAmerica);
            var greenland = new Territory("GREENLAND", Continent.NorthAmerica);
            var northwestTerritory = new Territory("NORTHWEST_TERRITORY", Continent.NorthAmerica);
            var ontario = new Territory("ONTARIO", Continent.NorthAmerica);
            var quebec = new Territory("QUEBEC", Continent.NorthAmerica);
            var westernUnitedStates = new Territory("WESTERN_UNITED_STATES", Continent.NorthAmerica);

            var argentina = new Territory("ARGENTINA", Continent.SouthAmerica);
            var brazil = new Territory("BRAZIL", Continent.SouthAmerica);
            var peru = new Territory("PERU", Continent.SouthAmerica);
            var venezuela = new Territory("VENEZUELA", Continent.SouthAmerica);

            var greatBritain = new Territory("GREAT_BRITAIN", Continent.Europe);
            var iceland = new Territory("ICELAND", Continent.Europe);
            var northernEurope = new Territory("NORTHERN_EUROPE", Continent.Europe);
            var scandinavia = new Territory("SCANDINAVIA", Continent.Europe);
            var southernEurope = new Territory("SOUTHERN_EUROPE", Continent.Europe);
            var ukraine = new Territory("UKRAINE", Continent.Europe);
            var westernEurope = new Territory("WESTERN_EUROPE", Continent.Europe);

            var congo = new Territory("CONGO", Continent.Africa);
            var eastAfrica = new Territory("EAST_AFRICA", Continent.Africa);
            var egypt = new Territory("EGYPT", Continent.Africa);
            var madagascar = new Territory("MADAGASCAR", Continent.Africa);
            var northAfrica = new Territory("NORTH_AFRICA", Continent.Africa);
            var southAfrica = new Territory("SOUTH_AFRICA", Continent.Africa);

            var afghanistan = new Territory("AFGHANISTAN", Continent.Asia);
            var china = new Territory("CHINA", Continent.Asia);
            var india = new Territory("INDIA", Continent.Asia);
            var irkutsk = new Territory("IRKUTSK", Continent.Asia);
            var japan = new Territory("JAPAN", Continent.Asia);
            var kamchatka = new Territory("KAMCHATKA", Continent.Asia);
            var middleEast = new Territory("MIDDLE_EAST", Continent.Asia);
            var mongolia = new Territory("MONGOLIA", Continent.Asia);
            var siam = new Territory("SIAM", Continent.Asia);
            var siberia = new Territory("SIBERIA", Continent.Asia);
            var ural = new Territory("URAL", Continent.Asia);
            var yakutsk = new Territory("YAKUTSK", Continent.Asia);

            var easternAustralia = new Territory("EASTERN_AUSTRALIA", Continent.Australia);
            var indonesia = new Territory("INDONESIA", Continent.Australia);
            var newGuinea = new Territory("NEW_GUINEA", Continent.Australia);
            var westernAustralia = new Territory("WESTERN_AUSTRALIA", Continent.Australia);

            alaska.AddBorderToTerritories(alberta, northwestTerritory, kamchatka);
            alberta.AddBorderToTerritories(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddBorderToTerritories(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddBorderToTerritories(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddBorderToTerritories(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddBorderToTerritories(alaska, alberta, greenland, ontario);
            ontario.AddBorderToTerritories(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddBorderToTerritories(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddBorderToTerritories(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddBorderToTerritories(brazil, peru);
            brazil.AddBorderToTerritories(argentina, peru, venezuela, northAfrica);
            peru.AddBorderToTerritories(argentina, brazil, venezuela);
            venezuela.AddBorderToTerritories(brazil, peru, centralAmerica);

            greatBritain.AddBorderToTerritories(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddBorderToTerritories(greatBritain, scandinavia, greenland);
            northernEurope.AddBorderToTerritories(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddBorderToTerritories(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddBorderToTerritories(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddBorderToTerritories(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddBorderToTerritories(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddBorderToTerritories(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddBorderToTerritories(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddBorderToTerritories(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddBorderToTerritories(eastAfrica, southAfrica);
            northAfrica.AddBorderToTerritories(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddBorderToTerritories(congo, eastAfrica, madagascar);

            afghanistan.AddBorderToTerritories(china, india, middleEast, ural, ukraine);
            china.AddBorderToTerritories(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddBorderToTerritories(afghanistan, china, middleEast, siam);
            irkutsk.AddBorderToTerritories(kamchatka, mongolia, siberia, yakutsk);
            japan.AddBorderToTerritories(kamchatka, mongolia);
            kamchatka.AddBorderToTerritories(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddBorderToTerritories(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddBorderToTerritories(china, irkutsk, japan, kamchatka, siberia);
            siam.AddBorderToTerritories(china, india, indonesia);
            siberia.AddBorderToTerritories(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddBorderToTerritories(afghanistan, china, siberia, ukraine);
            yakutsk.AddBorderToTerritories(irkutsk, kamchatka, siberia);

            easternAustralia.AddBorderToTerritories(newGuinea, westernAustralia);
            indonesia.AddBorderToTerritories(newGuinea, westernAustralia, siam);
            newGuinea.AddBorderToTerritories(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddBorderToTerritories(easternAustralia, indonesia, newGuinea);

            Alaska = alaska;
            Alberta = alberta;
            CentralAmerica = centralAmerica;
            EasternUnitedStates = easternUnitedStates;
            Greenland = greenland;
            NorthwestTerritory = northwestTerritory;
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

        public IEnumerable<ITerritory> GetAll()
        {
            return new[]
            {
                Alaska,
                Alberta,
                CentralAmerica,
                EasternUnitedStates,
                Greenland,
                NorthwestTerritory,
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

        public ITerritory Alaska { get; }
        public ITerritory Alberta { get; }
        public ITerritory CentralAmerica { get; }
        public ITerritory EasternUnitedStates { get; }
        public ITerritory Greenland { get; }
        public ITerritory NorthwestTerritory { get; }
        public ITerritory Ontario { get; }
        public ITerritory Quebec { get; }
        public ITerritory WesternUnitedStates { get; }

        public ITerritory Argentina { get; }
        public ITerritory Brazil { get; }
        public ITerritory Peru { get; }
        public ITerritory Venezuela { get; }

        public ITerritory GreatBritain { get; }
        public ITerritory Iceland { get; }
        public ITerritory NorthernEurope { get; }
        public ITerritory Scandinavia { get; }
        public ITerritory SouthernEurope { get; }
        public ITerritory Ukraine { get; }
        public ITerritory WesternEurope { get; }

        public ITerritory Congo { get; }
        public ITerritory EastAfrica { get; }
        public ITerritory Egypt { get; }
        public ITerritory Madagascar { get; }
        public ITerritory NorthAfrica { get; }
        public ITerritory SouthAfrica { get; }

        public ITerritory Afghanistan { get; }
        public ITerritory China { get; }
        public ITerritory India { get; }
        public ITerritory Irkutsk { get; }
        public ITerritory Japan { get; }
        public ITerritory Kamchatka { get; }
        public ITerritory MiddleEast { get; }
        public ITerritory Mongolia { get; }
        public ITerritory Siam { get; }
        public ITerritory Siberia { get; }
        public ITerritory Ural { get; }
        public ITerritory Yakutsk { get; }

        public ITerritory EasternAustralia { get; }
        public ITerritory Indonesia { get; }
        public ITerritory NewGuinea { get; }
        public ITerritory WesternAustralia { get; }
    }
}