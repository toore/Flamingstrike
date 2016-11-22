﻿using System.Collections.Generic;

namespace FlamingStrike.GameEngine
{
    public interface IRegions
    {
        IEnumerable<IRegion> GetAll();

        IRegion Alaska { get; }
        IRegion Alberta { get; }
        IRegion CentralAmerica { get; }
        IRegion EasternUnitedStates { get; }
        IRegion Greenland { get; }
        IRegion NorthwestRegion { get; }
        IRegion Ontario { get; }
        IRegion Quebec { get; }
        IRegion WesternUnitedStates { get; }
        IRegion Argentina { get; }
        IRegion Brazil { get; }
        IRegion Peru { get; }
        IRegion Venezuela { get; }
        IRegion GreatBritain { get; }
        IRegion Iceland { get; }
        IRegion NorthernEurope { get; }
        IRegion Scandinavia { get; }
        IRegion SouthernEurope { get; }
        IRegion Ukraine { get; }
        IRegion WesternEurope { get; }
        IRegion Congo { get; }
        IRegion EastAfrica { get; }
        IRegion Egypt { get; }
        IRegion Madagascar { get; }
        IRegion NorthAfrica { get; }
        IRegion SouthAfrica { get; }
        IRegion Afghanistan { get; }
        IRegion China { get; }
        IRegion India { get; }
        IRegion Irkutsk { get; }
        IRegion Japan { get; }
        IRegion Kamchatka { get; }
        IRegion MiddleEast { get; }
        IRegion Mongolia { get; }
        IRegion Siam { get; }
        IRegion Siberia { get; }
        IRegion Ural { get; }
        IRegion Yakutsk { get; }
        IRegion EasternAustralia { get; }
        IRegion Indonesia { get; }
        IRegion NewGuinea { get; }
        IRegion WesternAustralia { get; }
    }

    public class Regions : IRegions
    {
        public Regions(IContinents continents)
        {
            var alaska = new Region(continents.NorthAmerica);
            var alberta = new Region(continents.NorthAmerica);
            var centralAmerica = new Region(continents.NorthAmerica);
            var easternUnitedStates = new Region(continents.NorthAmerica);
            var greenland = new Region(continents.NorthAmerica);
            var northwestTerritory = new Region(continents.NorthAmerica);
            var ontario = new Region(continents.NorthAmerica);
            var quebec = new Region(continents.NorthAmerica);
            var westernUnitedStates = new Region(continents.NorthAmerica);

            var argentina = new Region(continents.SouthAmerica);
            var brazil = new Region(continents.SouthAmerica);
            var peru = new Region(continents.SouthAmerica);
            var venezuela = new Region(continents.SouthAmerica);

            var greatBritain = new Region(continents.Europe);
            var iceland = new Region(continents.Europe);
            var northernEurope = new Region(continents.Europe);
            var scandinavia = new Region(continents.Europe);
            var southernEurope = new Region(continents.Europe);
            var ukraine = new Region(continents.Europe);
            var westernEurope = new Region(continents.Europe);

            var congo = new Region(continents.Africa);
            var eastAfrica = new Region(continents.Africa);
            var egypt = new Region(continents.Africa);
            var madagascar = new Region(continents.Africa);
            var northAfrica = new Region(continents.Africa);
            var southAfrica = new Region(continents.Africa);

            var afghanistan = new Region(continents.Asia);
            var china = new Region(continents.Asia);
            var india = new Region(continents.Asia);
            var irkutsk = new Region(continents.Asia);
            var japan = new Region(continents.Asia);
            var kamchatka = new Region(continents.Asia);
            var middleEast = new Region(continents.Asia);
            var mongolia = new Region(continents.Asia);
            var siam = new Region(continents.Asia);
            var siberia = new Region(continents.Asia);
            var ural = new Region(continents.Asia);
            var yakutsk = new Region(continents.Asia);

            var easternAustralia = new Region(continents.Australia);
            var indonesia = new Region(continents.Australia);
            var newGuinea = new Region(continents.Australia);
            var westernAustralia = new Region(continents.Australia);

            alaska.AddBordersToRegions(alberta, northwestTerritory, kamchatka);
            alberta.AddBordersToRegions(alaska, northwestTerritory, ontario, westernUnitedStates);
            centralAmerica.AddBordersToRegions(easternUnitedStates, westernUnitedStates, venezuela);
            easternUnitedStates.AddBordersToRegions(centralAmerica, ontario, quebec, westernUnitedStates);
            greenland.AddBordersToRegions(northwestTerritory, ontario, quebec, iceland);
            northwestTerritory.AddBordersToRegions(alaska, alberta, greenland, ontario);
            ontario.AddBordersToRegions(alberta, easternUnitedStates, greenland, northwestTerritory, quebec, westernUnitedStates);
            quebec.AddBordersToRegions(easternUnitedStates, greenland, ontario);
            westernUnitedStates.AddBordersToRegions(alberta, centralAmerica, easternUnitedStates, ontario);

            argentina.AddBordersToRegions(brazil, peru);
            brazil.AddBordersToRegions(argentina, peru, venezuela, northAfrica);
            peru.AddBordersToRegions(argentina, brazil, venezuela);
            venezuela.AddBordersToRegions(brazil, peru, centralAmerica);

            greatBritain.AddBordersToRegions(iceland, northernEurope, scandinavia, westernEurope);
            iceland.AddBordersToRegions(greatBritain, scandinavia, greenland);
            northernEurope.AddBordersToRegions(greatBritain, scandinavia, southernEurope, ukraine, westernEurope);
            scandinavia.AddBordersToRegions(greatBritain, iceland, northernEurope, ukraine);
            southernEurope.AddBordersToRegions(northernEurope, ukraine, westernEurope, egypt, northAfrica, middleEast);
            ukraine.AddBordersToRegions(northernEurope, scandinavia, southernEurope, afghanistan, middleEast, ural);
            westernEurope.AddBordersToRegions(greatBritain, northernEurope, southernEurope, northAfrica);

            congo.AddBordersToRegions(eastAfrica, northAfrica, southAfrica);
            eastAfrica.AddBordersToRegions(congo, egypt, madagascar, northAfrica, southAfrica, middleEast);
            egypt.AddBordersToRegions(eastAfrica, northAfrica, southernEurope, middleEast);
            madagascar.AddBordersToRegions(eastAfrica, southAfrica);
            northAfrica.AddBordersToRegions(congo, eastAfrica, egypt, brazil, southernEurope, westernEurope);
            southAfrica.AddBordersToRegions(congo, eastAfrica, madagascar);

            afghanistan.AddBordersToRegions(china, india, middleEast, ural, ukraine);
            china.AddBordersToRegions(afghanistan, india, mongolia, siam, siberia, ural);
            india.AddBordersToRegions(afghanistan, china, middleEast, siam);
            irkutsk.AddBordersToRegions(kamchatka, mongolia, siberia, yakutsk);
            japan.AddBordersToRegions(kamchatka, mongolia);
            kamchatka.AddBordersToRegions(irkutsk, japan, mongolia, yakutsk, alaska);
            middleEast.AddBordersToRegions(afghanistan, india, southernEurope, ukraine, egypt, eastAfrica);
            mongolia.AddBordersToRegions(china, irkutsk, japan, kamchatka, siberia);
            siam.AddBordersToRegions(china, india, indonesia);
            siberia.AddBordersToRegions(china, irkutsk, mongolia, ural, yakutsk);
            ural.AddBordersToRegions(afghanistan, china, siberia, ukraine);
            yakutsk.AddBordersToRegions(irkutsk, kamchatka, siberia);

            easternAustralia.AddBordersToRegions(newGuinea, westernAustralia);
            indonesia.AddBordersToRegions(newGuinea, westernAustralia, siam);
            newGuinea.AddBordersToRegions(easternAustralia, indonesia, westernAustralia);
            westernAustralia.AddBordersToRegions(easternAustralia, indonesia, newGuinea);

            Alaska = alaska;
            Alberta = alberta;
            CentralAmerica = centralAmerica;
            EasternUnitedStates = easternUnitedStates;
            Greenland = greenland;
            NorthwestRegion = northwestTerritory;
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

        public IEnumerable<IRegion> GetAll()
        {
            return new[]
            {
                Alaska,
                Alberta,
                CentralAmerica,
                EasternUnitedStates,
                Greenland,
                NorthwestRegion,
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

        public IRegion Alaska { get; }
        public IRegion Alberta { get; }
        public IRegion CentralAmerica { get; }
        public IRegion EasternUnitedStates { get; }
        public IRegion Greenland { get; }
        public IRegion NorthwestRegion { get; }
        public IRegion Ontario { get; }
        public IRegion Quebec { get; }
        public IRegion WesternUnitedStates { get; }

        public IRegion Argentina { get; }
        public IRegion Brazil { get; }
        public IRegion Peru { get; }
        public IRegion Venezuela { get; }

        public IRegion GreatBritain { get; }
        public IRegion Iceland { get; }
        public IRegion NorthernEurope { get; }
        public IRegion Scandinavia { get; }
        public IRegion SouthernEurope { get; }
        public IRegion Ukraine { get; }
        public IRegion WesternEurope { get; }

        public IRegion Congo { get; }
        public IRegion EastAfrica { get; }
        public IRegion Egypt { get; }
        public IRegion Madagascar { get; }
        public IRegion NorthAfrica { get; }
        public IRegion SouthAfrica { get; }

        public IRegion Afghanistan { get; }
        public IRegion China { get; }
        public IRegion India { get; }
        public IRegion Irkutsk { get; }
        public IRegion Japan { get; }
        public IRegion Kamchatka { get; }
        public IRegion MiddleEast { get; }
        public IRegion Mongolia { get; }
        public IRegion Siam { get; }
        public IRegion Siberia { get; }
        public IRegion Ural { get; }
        public IRegion Yakutsk { get; }

        public IRegion EasternAustralia { get; }
        public IRegion Indonesia { get; }
        public IRegion NewGuinea { get; }
        public IRegion WesternAustralia { get; }
    }
}