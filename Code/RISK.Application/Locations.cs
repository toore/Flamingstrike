using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain
{
    public class Locations
    {
        public Locations()
        {
            var alaska = new Location("ALASKA", Continents.NorthAmerica);
            var alberta = new Location("ALBERTA", Continents.NorthAmerica);
            var centralAmerica = new Location("CENTRAL_AMERICA", Continents.NorthAmerica);
            var easternUnitedStates = new Location("EASTERN_UNITED_STATES", Continents.NorthAmerica);
            var greenland = new Location("GREENLAND", Continents.NorthAmerica);
            var northwestTerritory = new Location("NORTHWEST_TERRITORY", Continents.NorthAmerica);
            var ontario = new Location("ONTARIO", Continents.NorthAmerica);
            var quebec = new Location("QUEBEC", Continents.NorthAmerica);
            var westernUnitedStates = new Location("WESTERN_UNITED_STATES", Continents.NorthAmerica);

            var argentina = new Location("ARGENTINA", Continents.SouthAmerica);
            var brazil = new Location("BRAZIL", Continents.SouthAmerica);
            var peru = new Location("PERU", Continents.SouthAmerica);
            var venezuela = new Location("VENEZUELA", Continents.SouthAmerica);

            var greatBritain = new Location("GREAT_BRITAIN", Continents.Europe);
            var iceland = new Location("ICELAND", Continents.Europe);
            var northernEurope = new Location("NORTHERN_EUROPE", Continents.Europe);
            var scandinavia = new Location("SCANDINAVIA", Continents.Europe);
            var southernEurope = new Location("SOUTHERN_EUROPE", Continents.Europe);
            var ukraine = new Location("UKRAINE", Continents.Europe);
            var westernEurope = new Location("WESTERN_EUROPE", Continents.Europe);

            var congo = new Location("CONGO", Continents.Africa);
            var eastAfrica = new Location("EAST_AFRICA", Continents.Africa);
            var egypt = new Location("EGYPT", Continents.Africa);
            var madagascar = new Location("MADAGASCAR", Continents.Africa);
            var northAfrica = new Location("NORTH_AFRICA", Continents.Africa);
            var southAfrica = new Location("SOUTH_AFRICA", Continents.Africa);

            var afghanistan = new Location("AFGHANISTAN", Continents.Asia);
            var china = new Location("CHINA", Continents.Asia);
            var india = new Location("INDIA", Continents.Asia);
            var irkutsk = new Location("IRKUTSK", Continents.Asia);
            var japan = new Location("JAPAN", Continents.Asia);
            var kamchatka = new Location("KAMCHATKA", Continents.Asia);
            var middleEast = new Location("MIDDLE_EAST", Continents.Asia);
            var mongolia = new Location("MONGOLIA", Continents.Asia);
            var siam = new Location("SIAM", Continents.Asia);
            var siberia = new Location("SIBERIA", Continents.Asia);
            var ural = new Location("URAL", Continents.Asia);
            var yakutsk = new Location("YAKUTSK", Continents.Asia);

            var easternAustralia = new Location("EASTERN_AUSTRALIA", Continents.Australia);
            var indonesia = new Location("INDONESIA", Continents.Australia);
            var newGuinea = new Location("NEW_GUINEA", Continents.Australia);
            var westernAustralia = new Location("WESTERN_AUSTRALIA", Continents.Australia);

            alaska.AddBorderingTerritories(alberta, northwestTerritory, kamchatka);
            alberta.AddBorderingTerritories(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddBorderingTerritories(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddBorderingTerritories(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddBorderingTerritories(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddBorderingTerritories(alaska, alberta, greenland, ontario);
            ontario.AddBorderingTerritories(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddBorderingTerritories(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddBorderingTerritories(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddBorderingTerritories(brazil, peru);
            brazil.AddBorderingTerritories(argentina, peru, venezuela, northAfrica);
            peru.AddBorderingTerritories(argentina, brazil, venezuela);
            venezuela.AddBorderingTerritories(brazil, peru, centralAmerica);

            greatBritain.AddBorderingTerritories(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddBorderingTerritories(greatBritain, scandinavia, greenland);
            northernEurope.AddBorderingTerritories(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddBorderingTerritories(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddBorderingTerritories(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddBorderingTerritories(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddBorderingTerritories(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddBorderingTerritories(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddBorderingTerritories(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddBorderingTerritories(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddBorderingTerritories(eastAfrica, southAfrica);
            northAfrica.AddBorderingTerritories(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddBorderingTerritories(congo, eastAfrica, madagascar);

            afghanistan.AddBorderingTerritories(china, india, middleEast, ural, ukraine);
            china.AddBorderingTerritories(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddBorderingTerritories(afghanistan, china, middleEast, siam);
            irkutsk.AddBorderingTerritories(kamchatka, mongolia, siberia, yakutsk);
            japan.AddBorderingTerritories(kamchatka, mongolia);
            kamchatka.AddBorderingTerritories(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddBorderingTerritories(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddBorderingTerritories(china, irkutsk, japan, kamchatka, siberia);
            siam.AddBorderingTerritories(china, india, indonesia);
            siberia.AddBorderingTerritories(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddBorderingTerritories(afghanistan, china, siberia, ukraine);
            yakutsk.AddBorderingTerritories(irkutsk, kamchatka, siberia);

            easternAustralia.AddBorderingTerritories(newGuinea, westernAustralia);
            indonesia.AddBorderingTerritories(newGuinea, westernAustralia, siam);
            newGuinea.AddBorderingTerritories(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddBorderingTerritories(easternAustralia, indonesia, newGuinea);

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