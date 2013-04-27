﻿using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class LocationProvider : ILocationProvider
    {
        private readonly IContinentProvider _continentProvider;

        public LocationProvider(IContinentProvider continentProvider)
        {
            _continentProvider = continentProvider;

            var alaska = new Location("ALASKA", _continentProvider.NorthAmerica);
            var alberta = new Location("ALBERTA", _continentProvider.NorthAmerica);
            var centralAmerica = new Location("CENTRAL_AMERICA", _continentProvider.NorthAmerica);
            var easternUnitedStates = new Location("EASTERN_UNITED_STATES", _continentProvider.NorthAmerica);
            var greenland = new Location("GREENLAND", _continentProvider.NorthAmerica);
            var northwestTerritory = new Location("NORTHWEST_TERRITORY", _continentProvider.NorthAmerica);
            var ontario = new Location("ONTARIO", _continentProvider.NorthAmerica);
            var quebec = new Location("QUEBEC", _continentProvider.NorthAmerica);
            var westernUnitedStates = new Location("WESTERN_UNITED_STATES", _continentProvider.NorthAmerica);

            var argentina = new Location("ARGENTINA", _continentProvider.SouthAmerica);
            var brazil = new Location("BRAZIL", _continentProvider.SouthAmerica);
            var peru = new Location("PERU", _continentProvider.SouthAmerica);
            var venezuela = new Location("VENEZUELA", _continentProvider.SouthAmerica);

            var greatBritain = new Location("GREAT_BRITAIN", _continentProvider.Europe);
            var iceland = new Location("ICELAND", _continentProvider.Europe);
            var northernEurope = new Location("NORTHERN_EUROPE", _continentProvider.Europe);
            var scandinavia = new Location("SCANDINAVIA", _continentProvider.Europe);
            var southernEurope = new Location("SOUTHERN_EUROPE", _continentProvider.Europe);
            var ukraine = new Location("UKRAINE", _continentProvider.Europe);
            var westernEurope = new Location("WESTERN_EUROPE", _continentProvider.Europe);

            var congo = new Location("CONGO", _continentProvider.Africa);
            var eastAfrica = new Location("EAST_AFRICA", _continentProvider.Africa);
            var egypt = new Location("EGYPT", _continentProvider.Africa);
            var madagascar = new Location("MADAGASCAR", _continentProvider.Africa);
            var northAfrica = new Location("NORTH_AFRICA", _continentProvider.Africa);
            var southAfrica = new Location("SOUTH_AFRICA", _continentProvider.Africa);

            var afghanistan = new Location("AFGHANISTAN", _continentProvider.Asia);
            var china = new Location("CHINA", _continentProvider.Asia);
            var india = new Location("INDIA", _continentProvider.Asia);
            var irkutsk = new Location("IRKUTSK", _continentProvider.Asia);
            var japan = new Location("JAPAN", _continentProvider.Asia);
            var kamchatka = new Location("KAMCHATKA", _continentProvider.Asia);
            var middleEast = new Location("MIDDLE_EAST", _continentProvider.Asia);
            var mongolia = new Location("MONGOLIA", _continentProvider.Asia);
            var siam = new Location("SIAM", _continentProvider.Asia);
            var siberia = new Location("SIBERIA", _continentProvider.Asia);
            var ural = new Location("URAL", _continentProvider.Asia);
            var yakutsk = new Location("YAKUTSK", _continentProvider.Asia);

            var easternAustralia = new Location("EASTERN_AUSTRALIA", _continentProvider.Australia);
            var indonesia = new Location("INDONESIA", _continentProvider.Australia);
            var newGuinea = new Location("NEW_GUINEA", _continentProvider.Australia);
            var westernAustralia = new Location("WESTERN_AUSTRALIA", _continentProvider.Australia);

            alaska.AddConnectedTerritories(alberta, northwestTerritory, kamchatka);
            alberta.AddConnectedTerritories(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddConnectedTerritories(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddConnectedTerritories(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddConnectedTerritories(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddConnectedTerritories(alaska, alberta, greenland, ontario);
            ontario.AddConnectedTerritories(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddConnectedTerritories(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddConnectedTerritories(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddConnectedTerritories(brazil, peru);
            brazil.AddConnectedTerritories(argentina, peru, venezuela, northAfrica);
            peru.AddConnectedTerritories(argentina, brazil, venezuela);
            venezuela.AddConnectedTerritories(brazil, peru, centralAmerica);

            greatBritain.AddConnectedTerritories(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddConnectedTerritories(greatBritain, scandinavia, greenland);
            northernEurope.AddConnectedTerritories(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddConnectedTerritories(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddConnectedTerritories(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddConnectedTerritories(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddConnectedTerritories(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddConnectedTerritories(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddConnectedTerritories(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddConnectedTerritories(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddConnectedTerritories(eastAfrica, southAfrica);
            northAfrica.AddConnectedTerritories(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddConnectedTerritories(congo, eastAfrica, madagascar);

            afghanistan.AddConnectedTerritories(china, india, middleEast, ural, ukraine);
            china.AddConnectedTerritories(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddConnectedTerritories(afghanistan, china, middleEast, siam);
            irkutsk.AddConnectedTerritories(kamchatka, mongolia, siberia, yakutsk);
            japan.AddConnectedTerritories(kamchatka, mongolia);
            kamchatka.AddConnectedTerritories(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddConnectedTerritories(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddConnectedTerritories(china, irkutsk, japan, kamchatka, siberia);
            siam.AddConnectedTerritories(china, india, indonesia);
            siberia.AddConnectedTerritories(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddConnectedTerritories(afghanistan, china, siberia, ukraine);
            yakutsk.AddConnectedTerritories(irkutsk, kamchatka, siberia);

            easternAustralia.AddConnectedTerritories(newGuinea, westernAustralia);
            indonesia.AddConnectedTerritories(newGuinea, westernAustralia, siam);
            newGuinea.AddConnectedTerritories(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddConnectedTerritories(easternAustralia, indonesia, newGuinea);

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