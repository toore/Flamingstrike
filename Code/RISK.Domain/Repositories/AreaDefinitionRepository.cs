﻿using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class AreaDefinitionRepository : IAreaDefinitionRepository
    {
        private readonly IContinentRepository _continentRepository;

        public AreaDefinitionRepository(IContinentRepository continentRepository)
        {
            _continentRepository = continentRepository;

            var alaska = new AreaDefinition("ALASKA", _continentRepository.NorthAmerica);
            var alberta = new AreaDefinition("ALBERTA", _continentRepository.NorthAmerica);
            var centralAmerica = new AreaDefinition("CENTRAL_AMERICA", _continentRepository.NorthAmerica);
            var easternUnitedStates = new AreaDefinition("EASTERN_UNITED_STATES", _continentRepository.NorthAmerica);
            var greenland = new AreaDefinition("GREENLAND", _continentRepository.NorthAmerica);
            var northwestTerritory = new AreaDefinition("NORTHWEST_TERRITORY", _continentRepository.NorthAmerica);
            var ontario = new AreaDefinition("ONTARIO", _continentRepository.NorthAmerica);
            var quebec = new AreaDefinition("QUEBEC", _continentRepository.NorthAmerica);
            var westernUnitedStates = new AreaDefinition("WESTERN_UNITED_STATES", _continentRepository.NorthAmerica);

            var argentina = new AreaDefinition("ARGENTINA", _continentRepository.SouthAmerica);
            var brazil = new AreaDefinition("BRAZIL", _continentRepository.SouthAmerica);
            var peru = new AreaDefinition("PERU", _continentRepository.SouthAmerica);
            var venezuela = new AreaDefinition("VENEZUELA", _continentRepository.SouthAmerica);

            var greatBritain = new AreaDefinition("GREAT_BRITAIN", _continentRepository.Europe);
            var iceland = new AreaDefinition("ICELAND", _continentRepository.Europe);
            var northernEurope = new AreaDefinition("NORTHERN_EUROPE", _continentRepository.Europe);
            var scandinavia = new AreaDefinition("SCANDINAVIA", _continentRepository.Europe);
            var southernEurope = new AreaDefinition("SOUTHERN_EUROPE", _continentRepository.Europe);
            var ukraine = new AreaDefinition("UKRAINE", _continentRepository.Europe);
            var westernEurope = new AreaDefinition("WESTERN_EUROPE", _continentRepository.Europe);

            var congo = new AreaDefinition("CONGO", _continentRepository.Africa);
            var eastAfrica = new AreaDefinition("EAST_AFRICA", _continentRepository.Africa);
            var egypt = new AreaDefinition("EGYPT", _continentRepository.Africa);
            var madagascar = new AreaDefinition("MADAGASCAR", _continentRepository.Africa);
            var northAfrica = new AreaDefinition("NORTH_AFRICA", _continentRepository.Africa);
            var southAfrica = new AreaDefinition("SOUTH_AFRICA", _continentRepository.Africa);

            var afghanistan = new AreaDefinition("AFGHANISTAN", _continentRepository.Asia);
            var china = new AreaDefinition("CHINA", _continentRepository.Asia);
            var india = new AreaDefinition("INDIA", _continentRepository.Asia);
            var irkutsk = new AreaDefinition("IRKUTSK", _continentRepository.Asia);
            var japan = new AreaDefinition("JAPAN", _continentRepository.Asia);
            var kamchatka = new AreaDefinition("KAMCHATKA", _continentRepository.Asia);
            var middleEast = new AreaDefinition("MIDDLE_EAST", _continentRepository.Asia);
            var mongolia = new AreaDefinition("MONGOLIA", _continentRepository.Asia);
            var siam = new AreaDefinition("SIAM", _continentRepository.Asia);
            var siberia = new AreaDefinition("SIBERIA", _continentRepository.Asia);
            var ural = new AreaDefinition("URAL", _continentRepository.Asia);
            var yakutsk = new AreaDefinition("YAKUTSK", _continentRepository.Asia);

            var easternAustralia = new AreaDefinition("EASTERN_AUSTRALIA", _continentRepository.Australia);
            var indonesia = new AreaDefinition("INDONESIA", _continentRepository.Australia);
            var newGuinea = new AreaDefinition("NEW_GUINEA", _continentRepository.Australia);
            var westernAustralia = new AreaDefinition("WESTERN_AUSTRALIA", _continentRepository.Australia);

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

        public IEnumerable<IAreaDefinition> GetAll()
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

        public IAreaDefinition Alaska { get; private set; }
        public IAreaDefinition Alberta { get; private set; }
        public IAreaDefinition CentralAmerica { get; private set; }
        public IAreaDefinition EasternUnitedStates { get; private set; }
        public IAreaDefinition Greenland { get; private set; }
        public IAreaDefinition NorthwestTerritory { get; private set; }
        public IAreaDefinition Ontario { get; private set; }
        public IAreaDefinition Quebec { get; private set; }
        public IAreaDefinition WesternUnitedStates { get; private set; }

        public IAreaDefinition Argentina { get; private set; }
        public IAreaDefinition Brazil { get; private set; }
        public IAreaDefinition Peru { get; private set; }
        public IAreaDefinition Venezuela { get; private set; }

        public IAreaDefinition GreatBritain { get; private set; }
        public IAreaDefinition Iceland { get; private set; }
        public IAreaDefinition NorthernEurope { get; private set; }
        public IAreaDefinition Scandinavia { get; private set; }
        public IAreaDefinition SouthernEurope { get; private set; }
        public IAreaDefinition Ukraine { get; private set; }
        public IAreaDefinition WesternEurope { get; private set; }

        public IAreaDefinition Congo { get; private set; }
        public IAreaDefinition EastAfrica { get; private set; }
        public IAreaDefinition Egypt { get; private set; }
        public IAreaDefinition Madagascar { get; private set; }
        public IAreaDefinition NorthAfrica { get; private set; }
        public IAreaDefinition SouthAfrica { get; private set; }

        public IAreaDefinition Afghanistan { get; private set; }
        public IAreaDefinition China { get; private set; }
        public IAreaDefinition India { get; private set; }
        public IAreaDefinition Irkutsk { get; private set; }
        public IAreaDefinition Japan { get; private set; }
        public IAreaDefinition Kamchatka { get; private set; }
        public IAreaDefinition MiddleEast { get; private set; }
        public IAreaDefinition Mongolia { get; private set; }
        public IAreaDefinition Siam { get; private set; }
        public IAreaDefinition Siberia { get; private set; }
        public IAreaDefinition Ural { get; private set; }
        public IAreaDefinition Yakutsk { get; private set; }

        public IAreaDefinition EasternAustralia { get; private set; }
        public IAreaDefinition Indonesia { get; private set; }
        public IAreaDefinition NewGuinea { get; private set; }
        public IAreaDefinition WesternAustralia { get; private set; }
    }
}