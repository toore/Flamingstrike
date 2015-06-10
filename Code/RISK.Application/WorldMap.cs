using System.Collections.Generic;
using System.Linq;

namespace RISK.Application
{
    public interface IWorldMap
    {
        IEnumerable<ITerritory> GetTerritories();

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

        IEnumerable<ITerritory> GetTerritoriesOccupiedByPlayer(IPlayer player);
        IEnumerable<IPlayer> GetAllPlayersOccupyingTerritories();
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

        public IEnumerable<ITerritory> GetTerritories()
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

        public IEnumerable<ITerritory> GetTerritoriesOccupiedByPlayer(IPlayer player)
        {
            return GetTerritories()
                .Where(x => x.Occupant == player)
                .ToList();
        }

        public IEnumerable<IPlayer> GetAllPlayersOccupyingTerritories()
        {
            return GetTerritories()
                .Where(x => x.IsOccupied())
                .Select(x => x.Occupant)
                .Distinct()
                .ToList();
        }
    }
}