using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IContinentRepository _continentRepository;

        public LocationRepository(IContinentRepository continentRepository)
        {
            _continentRepository = continentRepository;

            var alaska = new Location("ALASKA", _continentRepository.NorthAmerica);
            var alberta = new Location("ALBERTA", _continentRepository.NorthAmerica);
            var centralAmerica = new Location("CENTRAL_AMERICA", _continentRepository.NorthAmerica);
            var easternUnitedStates = new Location("EASTERN_UNITED_STATES", _continentRepository.NorthAmerica);
            var greenland = new Location("GREENLAND", _continentRepository.NorthAmerica);
            var northwestTerritory = new Location("NORTHWEST_TERRITORY", _continentRepository.NorthAmerica);
            var ontario = new Location("ONTARIO", _continentRepository.NorthAmerica);
            var quebec = new Location("QUEBEC", _continentRepository.NorthAmerica);
            var westernUnitedStates = new Location("WESTERN_UNITED_STATES", _continentRepository.NorthAmerica);

            var argentina = new Location("ARGENTINA", _continentRepository.SouthAmerica);
            var brazil = new Location("BRAZIL", _continentRepository.SouthAmerica);
            var peru = new Location("PERU", _continentRepository.SouthAmerica);
            var venezuela = new Location("VENEZUELA", _continentRepository.SouthAmerica);

            var greatBritain = new Location("GREAT_BRITAIN", _continentRepository.Europe);
            var iceland = new Location("ICELAND", _continentRepository.Europe);
            var northernEurope = new Location("NORTHERN_EUROPE", _continentRepository.Europe);
            var scandinavia = new Location("SCANDINAVIA", _continentRepository.Europe);
            var southernEurope = new Location("SOUTHERN_EUROPE", _continentRepository.Europe);
            var ukraine = new Location("UKRAINE", _continentRepository.Europe);
            var westernEurope = new Location("WESTERN_EUROPE", _continentRepository.Europe);

            var congo = new Location("CONGO", _continentRepository.Africa);
            var eastAfrica = new Location("EAST_AFRICA", _continentRepository.Africa);
            var egypt = new Location("EGYPT", _continentRepository.Africa);
            var madagascar = new Location("MADAGASCAR", _continentRepository.Africa);
            var northAfrica = new Location("NORTH_AFRICA", _continentRepository.Africa);
            var southAfrica = new Location("SOUTH_AFRICA", _continentRepository.Africa);

            var afghanistan = new Location("AFGHANISTAN", _continentRepository.Asia);
            var china = new Location("CHINA", _continentRepository.Asia);
            var india = new Location("INDIA", _continentRepository.Asia);
            var irkutsk = new Location("IRKUTSK", _continentRepository.Asia);
            var japan = new Location("JAPAN", _continentRepository.Asia);
            var kamchatka = new Location("KAMCHATKA", _continentRepository.Asia);
            var middleEast = new Location("MIDDLE_EAST", _continentRepository.Asia);
            var mongolia = new Location("MONGOLIA", _continentRepository.Asia);
            var siam = new Location("SIAM", _continentRepository.Asia);
            var siberia = new Location("SIBERIA", _continentRepository.Asia);
            var ural = new Location("URAL", _continentRepository.Asia);
            var yakutsk = new Location("YAKUTSK", _continentRepository.Asia);

            var easternAustralia = new Location("EASTERN_AUSTRALIA", _continentRepository.Australia);
            var indonesia = new Location("INDONESIA", _continentRepository.Australia);
            var newGuinea = new Location("NEW_GUINEA", _continentRepository.Australia);
            var westernAustralia = new Location("WESTERN_AUSTRALIA", _continentRepository.Australia);

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