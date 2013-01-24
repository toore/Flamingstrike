﻿using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class TerritoryLocationRepository : ITerritoryLocationRepository
    {
        private readonly IContinentRepository _continentRepository;

        public TerritoryLocationRepository(IContinentRepository continentRepository)
        {
            _continentRepository = continentRepository;

            var alaska = new TerritoryLocation("ALASKA", _continentRepository.NorthAmerica);
            var alberta = new TerritoryLocation("ALBERTA", _continentRepository.NorthAmerica);
            var centralAmerica = new TerritoryLocation("CENTRAL_AMERICA", _continentRepository.NorthAmerica);
            var easternUnitedStates = new TerritoryLocation("EASTERN_UNITED_STATES", _continentRepository.NorthAmerica);
            var greenland = new TerritoryLocation("GREENLAND", _continentRepository.NorthAmerica);
            var northwestTerritory = new TerritoryLocation("NORTHWEST_TERRITORY", _continentRepository.NorthAmerica);
            var ontario = new TerritoryLocation("ONTARIO", _continentRepository.NorthAmerica);
            var quebec = new TerritoryLocation("QUEBEC", _continentRepository.NorthAmerica);
            var westernUnitedStates = new TerritoryLocation("WESTERN_UNITED_STATES", _continentRepository.NorthAmerica);

            var argentina = new TerritoryLocation("ARGENTINA", _continentRepository.SouthAmerica);
            var brazil = new TerritoryLocation("BRAZIL", _continentRepository.SouthAmerica);
            var peru = new TerritoryLocation("PERU", _continentRepository.SouthAmerica);
            var venezuela = new TerritoryLocation("VENEZUELA", _continentRepository.SouthAmerica);

            var greatBritain = new TerritoryLocation("GREAT_BRITAIN", _continentRepository.Europe);
            var iceland = new TerritoryLocation("ICELAND", _continentRepository.Europe);
            var northernEurope = new TerritoryLocation("NORTHERN_EUROPE", _continentRepository.Europe);
            var scandinavia = new TerritoryLocation("SCANDINAVIA", _continentRepository.Europe);
            var southernEurope = new TerritoryLocation("SOUTHERN_EUROPE", _continentRepository.Europe);
            var ukraine = new TerritoryLocation("UKRAINE", _continentRepository.Europe);
            var westernEurope = new TerritoryLocation("WESTERN_EUROPE", _continentRepository.Europe);

            var congo = new TerritoryLocation("CONGO", _continentRepository.Africa);
            var eastAfrica = new TerritoryLocation("EAST_AFRICA", _continentRepository.Africa);
            var egypt = new TerritoryLocation("EGYPT", _continentRepository.Africa);
            var madagascar = new TerritoryLocation("MADAGASCAR", _continentRepository.Africa);
            var northAfrica = new TerritoryLocation("NORTH_AFRICA", _continentRepository.Africa);
            var southAfrica = new TerritoryLocation("SOUTH_AFRICA", _continentRepository.Africa);

            var afghanistan = new TerritoryLocation("AFGHANISTAN", _continentRepository.Asia);
            var china = new TerritoryLocation("CHINA", _continentRepository.Asia);
            var india = new TerritoryLocation("INDIA", _continentRepository.Asia);
            var irkutsk = new TerritoryLocation("IRKUTSK", _continentRepository.Asia);
            var japan = new TerritoryLocation("JAPAN", _continentRepository.Asia);
            var kamchatka = new TerritoryLocation("KAMCHATKA", _continentRepository.Asia);
            var middleEast = new TerritoryLocation("MIDDLE_EAST", _continentRepository.Asia);
            var mongolia = new TerritoryLocation("MONGOLIA", _continentRepository.Asia);
            var siam = new TerritoryLocation("SIAM", _continentRepository.Asia);
            var siberia = new TerritoryLocation("SIBERIA", _continentRepository.Asia);
            var ural = new TerritoryLocation("URAL", _continentRepository.Asia);
            var yakutsk = new TerritoryLocation("YAKUTSK", _continentRepository.Asia);

            var easternAustralia = new TerritoryLocation("EASTERN_AUSTRALIA", _continentRepository.Australia);
            var indonesia = new TerritoryLocation("INDONESIA", _continentRepository.Australia);
            var newGuinea = new TerritoryLocation("NEW_GUINEA", _continentRepository.Australia);
            var westernAustralia = new TerritoryLocation("WESTERN_AUSTRALIA", _continentRepository.Australia);

            alaska.AddConnectedAreas(alberta, northwestTerritory, kamchatka);
            alberta.AddConnectedAreas(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddConnectedAreas(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddConnectedAreas(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddConnectedAreas(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddConnectedAreas(alaska, alberta, greenland, ontario);
            ontario.AddConnectedAreas(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddConnectedAreas(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddConnectedAreas(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddConnectedAreas(brazil, peru);
            brazil.AddConnectedAreas(argentina, peru, venezuela, northAfrica);
            peru.AddConnectedAreas(argentina, brazil, venezuela);
            venezuela.AddConnectedAreas(brazil, peru, centralAmerica);

            greatBritain.AddConnectedAreas(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddConnectedAreas(greatBritain, scandinavia, greenland);
            northernEurope.AddConnectedAreas(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddConnectedAreas(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddConnectedAreas(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddConnectedAreas(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddConnectedAreas(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddConnectedAreas(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddConnectedAreas(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddConnectedAreas(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddConnectedAreas(eastAfrica, southAfrica);
            northAfrica.AddConnectedAreas(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddConnectedAreas(congo, eastAfrica, madagascar);

            afghanistan.AddConnectedAreas(china, india, middleEast, ural, ukraine);
            china.AddConnectedAreas(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddConnectedAreas(afghanistan, china, middleEast, siam);
            irkutsk.AddConnectedAreas(kamchatka, mongolia, siberia, yakutsk);
            japan.AddConnectedAreas(kamchatka, mongolia);
            kamchatka.AddConnectedAreas(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddConnectedAreas(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddConnectedAreas(china, irkutsk, japan, kamchatka, siberia);
            siam.AddConnectedAreas(china, india, indonesia);
            siberia.AddConnectedAreas(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddConnectedAreas(afghanistan, china, siberia, ukraine);
            yakutsk.AddConnectedAreas(irkutsk, kamchatka, siberia);

            easternAustralia.AddConnectedAreas(newGuinea, westernAustralia);
            indonesia.AddConnectedAreas(newGuinea, westernAustralia, siam);
            newGuinea.AddConnectedAreas(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddConnectedAreas(easternAustralia, indonesia, newGuinea);

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

        public IEnumerable<ITerritoryLocation> GetAll()
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

        public ITerritoryLocation Alaska { get; private set; }
        public ITerritoryLocation Alberta { get; private set; }
        public ITerritoryLocation CentralAmerica { get; private set; }
        public ITerritoryLocation EasternUnitedStates { get; private set; }
        public ITerritoryLocation Greenland { get; private set; }
        public ITerritoryLocation NorthwestTerritory { get; private set; }
        public ITerritoryLocation Ontario { get; private set; }
        public ITerritoryLocation Quebec { get; private set; }
        public ITerritoryLocation WesternUnitedStates { get; private set; }

        public ITerritoryLocation Argentina { get; private set; }
        public ITerritoryLocation Brazil { get; private set; }
        public ITerritoryLocation Peru { get; private set; }
        public ITerritoryLocation Venezuela { get; private set; }

        public ITerritoryLocation GreatBritain { get; private set; }
        public ITerritoryLocation Iceland { get; private set; }
        public ITerritoryLocation NorthernEurope { get; private set; }
        public ITerritoryLocation Scandinavia { get; private set; }
        public ITerritoryLocation SouthernEurope { get; private set; }
        public ITerritoryLocation Ukraine { get; private set; }
        public ITerritoryLocation WesternEurope { get; private set; }

        public ITerritoryLocation Congo { get; private set; }
        public ITerritoryLocation EastAfrica { get; private set; }
        public ITerritoryLocation Egypt { get; private set; }
        public ITerritoryLocation Madagascar { get; private set; }
        public ITerritoryLocation NorthAfrica { get; private set; }
        public ITerritoryLocation SouthAfrica { get; private set; }

        public ITerritoryLocation Afghanistan { get; private set; }
        public ITerritoryLocation China { get; private set; }
        public ITerritoryLocation India { get; private set; }
        public ITerritoryLocation Irkutsk { get; private set; }
        public ITerritoryLocation Japan { get; private set; }
        public ITerritoryLocation Kamchatka { get; private set; }
        public ITerritoryLocation MiddleEast { get; private set; }
        public ITerritoryLocation Mongolia { get; private set; }
        public ITerritoryLocation Siam { get; private set; }
        public ITerritoryLocation Siberia { get; private set; }
        public ITerritoryLocation Ural { get; private set; }
        public ITerritoryLocation Yakutsk { get; private set; }

        public ITerritoryLocation EasternAustralia { get; private set; }
        public ITerritoryLocation Indonesia { get; private set; }
        public ITerritoryLocation NewGuinea { get; private set; }
        public ITerritoryLocation WesternAustralia { get; private set; }
    }
}