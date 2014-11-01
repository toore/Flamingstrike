using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain
{
    public class Locations
    {
        public Locations()
        {
            var alaska = new Location("ALASKA", Continent.NorthAmerica);
            var alberta = new Location("ALBERTA", Continent.NorthAmerica);
            var centralAmerica = new Location("CENTRAL_AMERICA", Continent.NorthAmerica);
            var easternUnitedStates = new Location("EASTERN_UNITED_STATES", Continent.NorthAmerica);
            var greenland = new Location("GREENLAND", Continent.NorthAmerica);
            var northwestTerritory = new Location("NORTHWEST_TERRITORY", Continent.NorthAmerica);
            var ontario = new Location("ONTARIO", Continent.NorthAmerica);
            var quebec = new Location("QUEBEC", Continent.NorthAmerica);
            var westernUnitedStates = new Location("WESTERN_UNITED_STATES", Continent.NorthAmerica);

            var argentina = new Location("ARGENTINA", Continent.SouthAmerica);
            var brazil = new Location("BRAZIL", Continent.SouthAmerica);
            var peru = new Location("PERU", Continent.SouthAmerica);
            var venezuela = new Location("VENEZUELA", Continent.SouthAmerica);

            var greatBritain = new Location("GREAT_BRITAIN", Continent.Europe);
            var iceland = new Location("ICELAND", Continent.Europe);
            var northernEurope = new Location("NORTHERN_EUROPE", Continent.Europe);
            var scandinavia = new Location("SCANDINAVIA", Continent.Europe);
            var southernEurope = new Location("SOUTHERN_EUROPE", Continent.Europe);
            var ukraine = new Location("UKRAINE", Continent.Europe);
            var westernEurope = new Location("WESTERN_EUROPE", Continent.Europe);

            var congo = new Location("CONGO", Continent.Africa);
            var eastAfrica = new Location("EAST_AFRICA", Continent.Africa);
            var egypt = new Location("EGYPT", Continent.Africa);
            var madagascar = new Location("MADAGASCAR", Continent.Africa);
            var northAfrica = new Location("NORTH_AFRICA", Continent.Africa);
            var southAfrica = new Location("SOUTH_AFRICA", Continent.Africa);

            var afghanistan = new Location("AFGHANISTAN", Continent.Asia);
            var china = new Location("CHINA", Continent.Asia);
            var india = new Location("INDIA", Continent.Asia);
            var irkutsk = new Location("IRKUTSK", Continent.Asia);
            var japan = new Location("JAPAN", Continent.Asia);
            var kamchatka = new Location("KAMCHATKA", Continent.Asia);
            var middleEast = new Location("MIDDLE_EAST", Continent.Asia);
            var mongolia = new Location("MONGOLIA", Continent.Asia);
            var siam = new Location("SIAM", Continent.Asia);
            var siberia = new Location("SIBERIA", Continent.Asia);
            var ural = new Location("URAL", Continent.Asia);
            var yakutsk = new Location("YAKUTSK", Continent.Asia);

            var easternAustralia = new Location("EASTERN_AUSTRALIA", Continent.Australia);
            var indonesia = new Location("INDONESIA", Continent.Australia);
            var newGuinea = new Location("NEW_GUINEA", Continent.Australia);
            var westernAustralia = new Location("WESTERN_AUSTRALIA", Continent.Australia);

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

        public IEnumerable<ILocation> GetAll()
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

        public ILocation Alaska { get; private set; }
        public ILocation Alberta { get; private set; }
        public ILocation CentralAmerica { get; private set; }
        public ILocation EasternUnitedStates { get; private set; }
        public ILocation Greenland { get; private set; }
        public ILocation NorthwestTerritory { get; private set; }
        public ILocation Ontario { get; private set; }
        public ILocation Quebec { get; private set; }
        public ILocation WesternUnitedStates { get; private set; }

        public ILocation Argentina { get; private set; }
        public ILocation Brazil { get; private set; }
        public ILocation Peru { get; private set; }
        public ILocation Venezuela { get; private set; }

        public ILocation GreatBritain { get; private set; }
        public ILocation Iceland { get; private set; }
        public ILocation NorthernEurope { get; private set; }
        public ILocation Scandinavia { get; private set; }
        public ILocation SouthernEurope { get; private set; }
        public ILocation Ukraine { get; private set; }
        public ILocation WesternEurope { get; private set; }

        public ILocation Congo { get; private set; }
        public ILocation EastAfrica { get; private set; }
        public ILocation Egypt { get; private set; }
        public ILocation Madagascar { get; private set; }
        public ILocation NorthAfrica { get; private set; }
        public ILocation SouthAfrica { get; private set; }

        public ILocation Afghanistan { get; private set; }
        public ILocation China { get; private set; }
        public ILocation India { get; private set; }
        public ILocation Irkutsk { get; private set; }
        public ILocation Japan { get; private set; }
        public ILocation Kamchatka { get; private set; }
        public ILocation MiddleEast { get; private set; }
        public ILocation Mongolia { get; private set; }
        public ILocation Siam { get; private set; }
        public ILocation Siberia { get; private set; }
        public ILocation Ural { get; private set; }
        public ILocation Yakutsk { get; private set; }

        public ILocation EasternAustralia { get; private set; }
        public ILocation Indonesia { get; private set; }
        public ILocation NewGuinea { get; private set; }
        public ILocation WesternAustralia { get; private set; }
    }
}