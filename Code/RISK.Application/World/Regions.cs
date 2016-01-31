﻿using System.Collections.Generic;

namespace RISK.Application.World
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
        public Regions()
        {
            var alaska = new Region(Continent.NorthAmerica);
            var alberta = new Region(Continent.NorthAmerica);
            var centralAmerica = new Region(Continent.NorthAmerica);
            var easternUnitedStates = new Region(Continent.NorthAmerica);
            var greenland = new Region(Continent.NorthAmerica);
            var northwestTerritory = new Region(Continent.NorthAmerica);
            var ontario = new Region(Continent.NorthAmerica);
            var quebec = new Region(Continent.NorthAmerica);
            var westernUnitedStates = new Region(Continent.NorthAmerica);

            var argentina = new Region(Continent.SouthAmerica);
            var brazil = new Region(Continent.SouthAmerica);
            var peru = new Region(Continent.SouthAmerica);
            var venezuela = new Region(Continent.SouthAmerica);

            var greatBritain = new Region(Continent.Europe);
            var iceland = new Region(Continent.Europe);
            var northernEurope = new Region(Continent.Europe);
            var scandinavia = new Region(Continent.Europe);
            var southernEurope = new Region(Continent.Europe);
            var ukraine = new Region(Continent.Europe);
            var westernEurope = new Region(Continent.Europe);

            var congo = new Region(Continent.Africa);
            var eastAfrica = new Region(Continent.Africa);
            var egypt = new Region(Continent.Africa);
            var madagascar = new Region(Continent.Africa);
            var northAfrica = new Region(Continent.Africa);
            var southAfrica = new Region(Continent.Africa);

            var afghanistan = new Region(Continent.Asia);
            var china = new Region(Continent.Asia);
            var india = new Region(Continent.Asia);
            var irkutsk = new Region(Continent.Asia);
            var japan = new Region(Continent.Asia);
            var kamchatka = new Region(Continent.Asia);
            var middleEast = new Region(Continent.Asia);
            var mongolia = new Region(Continent.Asia);
            var siam = new Region(Continent.Asia);
            var siberia = new Region(Continent.Asia);
            var ural = new Region(Continent.Asia);
            var yakutsk = new Region(Continent.Asia);

            var easternAustralia = new Region(Continent.Australia);
            var indonesia = new Region(Continent.Australia);
            var newGuinea = new Region(Continent.Australia);
            var westernAustralia = new Region(Continent.Australia);

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