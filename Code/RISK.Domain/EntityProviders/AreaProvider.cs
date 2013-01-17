using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.EntityProviders
{
    public class AreaProvider : IAreaProvider
    {
        private readonly IContinentProvider _continentProvider;

        public AreaProvider(IContinentProvider continentProvider)
        {
            _continentProvider = continentProvider;

            var alaska = new Area("ALASKA", _continentProvider.NorthAmerica);
            var alberta = new Area("ALBERTA", _continentProvider.NorthAmerica);
            var centralAmerica = new Area("CENTRAL_AMERICA", _continentProvider.NorthAmerica);
            var easternUnitedStates = new Area("EASTERN_UNITED_STATES", _continentProvider.NorthAmerica);
            var greenland = new Area("GREENLAND", _continentProvider.NorthAmerica);
            var northwestTerritory = new Area("NORTHWEST_TERRITORY", _continentProvider.NorthAmerica);
            var ontario = new Area("ONTARIO", _continentProvider.NorthAmerica);
            var quebec = new Area("QUEBEC", _continentProvider.NorthAmerica);
            var westernUnitedStates = new Area("WESTERN_UNITED_STATES", _continentProvider.NorthAmerica);

            var argentina = new Area("ARGENTINA", _continentProvider.SouthAmerica);
            var brazil = new Area("BRAZIL", _continentProvider.SouthAmerica);
            var peru = new Area("PERU", _continentProvider.SouthAmerica);
            var venezuela = new Area("VENEZUELA", _continentProvider.SouthAmerica);

            var greatBritain = new Area("GREAT_BRITAIN", _continentProvider.Europe);
            var iceland = new Area("ICELAND", _continentProvider.Europe);
            var northernEurope = new Area("NORTHERN_EUROPE", _continentProvider.Europe);
            var scandinavia = new Area("SCANDINAVIA", _continentProvider.Europe);
            var southernEurope = new Area("SOUTHERN_EUROPE", _continentProvider.Europe);
            var ukraine = new Area("UKRAINE", _continentProvider.Europe);
            var westernEurope = new Area("WESTERN_EUROPE", _continentProvider.Europe);

            var congo = new Area("CONGO", _continentProvider.Africa);
            var eastAfrica = new Area("EAST_AFRICA", _continentProvider.Africa);
            var egypt = new Area("EGYPT", _continentProvider.Africa);
            var madagascar = new Area("MADAGASCAR", _continentProvider.Africa);
            var northAfrica = new Area("NORTH_AFRICA", _continentProvider.Africa);
            var southAfrica = new Area("SOUTH_AFRICA", _continentProvider.Africa);

            var afghanistan = new Area("AFGHANISTAN", _continentProvider.Asia);
            var china = new Area("CHINA", _continentProvider.Asia);
            var india = new Area("INDIA", _continentProvider.Asia);
            var irkutsk = new Area("IRKUTSK", _continentProvider.Asia);
            var japan = new Area("JAPAN", _continentProvider.Asia);
            var kamchatka = new Area("KAMCHATKA", _continentProvider.Asia);
            var middleEast = new Area("MIDDLE_EAST", _continentProvider.Asia);
            var mongolia = new Area("MONGOLIA", _continentProvider.Asia);
            var siam = new Area("SIAM", _continentProvider.Asia);
            var siberia = new Area("SIBERIA", _continentProvider.Asia);
            var ural = new Area("URAL", _continentProvider.Asia);
            var yakutsk = new Area("YAKUTSK", _continentProvider.Asia);

            var easternAustralia = new Area("EASTERN_AUSTRALIA", _continentProvider.Australia);
            var indonesia = new Area("INDONESIA", _continentProvider.Australia);
            var newGuinea = new Area("NEW_GUINEA", _continentProvider.Australia);
            var westernAustralia = new Area("WESTERN_AUSTRALIA", _continentProvider.Australia);

            alaska.AddNeighbors(alberta, northwestTerritory, kamchatka);
            alberta.AddNeighbors(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddNeighbors(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddNeighbors(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddNeighbors(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddNeighbors(alaska, alberta, greenland, ontario);
            ontario.AddNeighbors(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddNeighbors(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddNeighbors(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddNeighbors(brazil, peru);
            brazil.AddNeighbors(argentina, peru, venezuela, northAfrica);
            peru.AddNeighbors(argentina, brazil, venezuela);
            venezuela.AddNeighbors(brazil, peru, centralAmerica);

            greatBritain.AddNeighbors(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddNeighbors(greatBritain, scandinavia, greenland);
            northernEurope.AddNeighbors(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddNeighbors(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddNeighbors(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddNeighbors(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddNeighbors(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddNeighbors(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddNeighbors(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddNeighbors(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddNeighbors(eastAfrica, southAfrica);
            northAfrica.AddNeighbors(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddNeighbors(congo, eastAfrica, madagascar);

            afghanistan.AddNeighbors(china, india, middleEast, ural, ukraine);
            china.AddNeighbors(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddNeighbors(afghanistan, china, middleEast, siam);
            irkutsk.AddNeighbors(kamchatka, mongolia, siberia, yakutsk);
            japan.AddNeighbors(kamchatka, mongolia);
            kamchatka.AddNeighbors(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddNeighbors(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddNeighbors(china, irkutsk, japan, kamchatka, siberia);
            siam.AddNeighbors(china, india, indonesia);
            siberia.AddNeighbors(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddNeighbors(afghanistan, china, siberia, ukraine);
            yakutsk.AddNeighbors(irkutsk, kamchatka, siberia);

            easternAustralia.AddNeighbors(newGuinea, westernAustralia);
            indonesia.AddNeighbors(newGuinea, westernAustralia, siam);
            newGuinea.AddNeighbors(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddNeighbors(easternAustralia, indonesia, newGuinea);

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

        public IEnumerable<IArea> GetAll()
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

        public IArea Alaska { get; private set; }
        public IArea Alberta { get; private set; }
        public IArea CentralAmerica { get; private set; }
        public IArea EasternUnitedStates { get; private set; }
        public IArea Greenland { get; private set; }
        public IArea NorthwestTerritory { get; private set; }
        public IArea Ontario { get; private set; }
        public IArea Quebec { get; private set; }
        public IArea WesternUnitedStates { get; private set; }

        public IArea Argentina { get; private set; }
        public IArea Brazil { get; private set; }
        public IArea Peru { get; private set; }
        public IArea Venezuela { get; private set; }

        public IArea GreatBritain { get; private set; }
        public IArea Iceland { get; private set; }
        public IArea NorthernEurope { get; private set; }
        public IArea Scandinavia { get; private set; }
        public IArea SouthernEurope { get; private set; }
        public IArea Ukraine { get; private set; }
        public IArea WesternEurope { get; private set; }

        public IArea Congo { get; private set; }
        public IArea EastAfrica { get; private set; }
        public IArea Egypt { get; private set; }
        public IArea Madagascar { get; private set; }
        public IArea NorthAfrica { get; private set; }
        public IArea SouthAfrica { get; private set; }

        public IArea Afghanistan { get; private set; }
        public IArea China { get; private set; }
        public IArea India { get; private set; }
        public IArea Irkutsk { get; private set; }
        public IArea Japan { get; private set; }
        public IArea Kamchatka { get; private set; }
        public IArea MiddleEast { get; private set; }
        public IArea Mongolia { get; private set; }
        public IArea Siam { get; private set; }
        public IArea Siberia { get; private set; }
        public IArea Ural { get; private set; }
        public IArea Yakutsk { get; private set; }

        public IArea EasternAustralia { get; private set; }
        public IArea Indonesia { get; private set; }
        public IArea NewGuinea { get; private set; }
        public IArea WesternAustralia { get; private set; }
    }
}