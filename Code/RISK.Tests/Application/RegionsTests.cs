using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public class RegionsTests
    {
        private readonly Regions _sut;
        private readonly Continents _continents;

        public RegionsTests()
        {
            _continents = new Continents();

            _sut = new Regions(_continents);
        }

        [Fact]
        public void Enumerating_all_returns_42()
        {
            _sut.Count().Should().Be(42);
            _sut.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Enumerating_all_contains_alaska()
        {
            _sut.Should().Contain(Alaska);
        }

        [Fact]
        public void Alaska_is_in_north_america()
        {
            Alaska.Continent.Should().Be(_continents.NorthAmerica);
        }

        [Fact]
        public void Alaskas_borders_alberta_and_northwest_territory_and_kamchatka()
        {
            AssertTerritoryBorders(Alaska, Alberta, Northwest, Kamchatka);
        }

        [Fact]
        public void Alberta_is_in_north_america_has_border_to_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(_continents.NorthAmerica);
            AssertTerritoryBorders(Alberta, Alaska, Northwest, Ontario, WesternUnitedStates);
        }

        [Fact]
        public void All_have_correct_borders()
        {
            AssertTerritoryBorders(Alaska, Alberta, Northwest, Kamchatka);
            AssertTerritoryBorders(Alberta, Alaska, Northwest, Ontario, WesternUnitedStates);
            AssertTerritoryBorders(CentralAmerica, EasternUnitedStates, WesternUnitedStates, Venezuela);
            AssertTerritoryBorders(EasternUnitedStates, CentralAmerica, Ontario, Quebec, WesternUnitedStates);
            AssertTerritoryBorders(Greenland, Northwest, Ontario, Quebec, Iceland);
            AssertTerritoryBorders(Northwest, Alaska, Alberta, Greenland, Ontario);
            AssertTerritoryBorders(Ontario, Alberta, EasternUnitedStates, Greenland, Northwest, Quebec, WesternUnitedStates);
            AssertTerritoryBorders(Quebec, EasternUnitedStates, Greenland, Ontario);
            AssertTerritoryBorders(WesternUnitedStates, Alberta, CentralAmerica, EasternUnitedStates, Ontario);

            AssertTerritoryBorders(Argentina, Brazil, Peru);
            AssertTerritoryBorders(Brazil, Argentina, Peru, Venezuela, NorthAfrica);
            AssertTerritoryBorders(Peru, Argentina, Brazil, Venezuela);
            AssertTerritoryBorders(Venezuela, Brazil, Peru, CentralAmerica);

            AssertTerritoryBorders(GreatBritain, Iceland, NorthernEurope, Scandinavia, WesternEurope);
            AssertTerritoryBorders(Iceland, GreatBritain, Scandinavia, Greenland);
            AssertTerritoryBorders(NorthernEurope, GreatBritain, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
            AssertTerritoryBorders(Scandinavia, GreatBritain, Iceland, NorthernEurope, Ukraine);
            AssertTerritoryBorders(SouthernEurope, NorthernEurope, Ukraine, WesternEurope, Egypt, NorthAfrica, MiddleEast);
            AssertTerritoryBorders(Ukraine, NorthernEurope, Scandinavia, SouthernEurope, Afghanistan, MiddleEast, Ural);
            AssertTerritoryBorders(WesternEurope, GreatBritain, NorthernEurope, SouthernEurope, NorthAfrica);

            AssertTerritoryBorders(Congo, EastAfrica, NorthAfrica, SouthAfrica);
            AssertTerritoryBorders(EastAfrica, Congo, Egypt, Madagascar, NorthAfrica, SouthAfrica, MiddleEast);
            AssertTerritoryBorders(Egypt, EastAfrica, NorthAfrica, SouthernEurope, MiddleEast);
            AssertTerritoryBorders(Madagascar, EastAfrica, SouthAfrica);
            AssertTerritoryBorders(NorthAfrica, Congo, EastAfrica, Egypt, Brazil, SouthernEurope, WesternEurope);
            AssertTerritoryBorders(SouthAfrica, Congo, EastAfrica, Madagascar);

            AssertTerritoryBorders(Afghanistan, China, India, MiddleEast, Ural, Ukraine);
            AssertTerritoryBorders(China, Afghanistan, India, Mongolia, Siam, Siberia, Ural);
            AssertTerritoryBorders(India, Afghanistan, China, MiddleEast, Siam);
            AssertTerritoryBorders(Irkutsk, Kamchatka, Mongolia, Siberia, Yakutsk);
            AssertTerritoryBorders(Japan, Kamchatka, Mongolia);
            AssertTerritoryBorders(Kamchatka, Irkutsk, Japan, Mongolia, Yakutsk, Alaska);
            AssertTerritoryBorders(MiddleEast, Afghanistan, India, SouthernEurope, Ukraine, EastAfrica, Egypt);
            AssertTerritoryBorders(Mongolia, China, Irkutsk, Japan, Kamchatka, Siberia);
            AssertTerritoryBorders(Siam, China, India, Indonesia);
            AssertTerritoryBorders(Siberia, China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertTerritoryBorders(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertTerritoryBorders(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertTerritoryBorders(EasternAustralia, NewGuinea, WesternAustralia);
            AssertTerritoryBorders(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertTerritoryBorders(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertTerritoryBorders(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
        }

        [Fact]
        public void North_america_has_9()
        {
            AssertTerritoriesInContinent(_continents.NorthAmerica, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, Northwest, Ontario, Quebec, WesternUnitedStates);
        }

        [Fact]
        public void South_america_has_4()
        {
            AssertTerritoriesInContinent(_continents.SouthAmerica, Argentina, Brazil, Peru, Venezuela);
        }

        [Fact]
        public void Europe_has_7()
        {
            AssertTerritoriesInContinent(_continents.Europe, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Fact]
        public void Africa_has_6()
        {
            AssertTerritoriesInContinent(_continents.Africa, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Fact]
        public void Asia_has_12()
        {
            AssertTerritoriesInContinent(_continents.Asia, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Fact]
        public void Australia_has_4()
        {
            AssertTerritoriesInContinent(_continents.Australia, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
        }

        [Fact]
        public void Enumerating_all_contains_all()
        {
            IEnumerable<IRegion> expected = new[]
            {
                Alaska,
                Alberta,
                CentralAmerica,
                EasternUnitedStates,
                Greenland,
                Northwest,
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

            _sut.Should().BeEquivalentTo(expected);
        }

        private void AssertTerritoryBorders(IRegion actual, params IRegion[] expectedItems)
        {
            foreach (var expected in expectedItems)
            {
                actual.HasBorder(expected).Should().BeTrue(actual + " should have border to " + expected);
                expected.HasBorder(actual).Should().BeTrue(expected + " should have border to " + actual);
            }
        }

        private void AssertTerritoriesInContinent(IContinent continent, params IRegion[] expected)
        {
            var actual = _sut
                .Where(x => x.Continent == continent)
                .ToList();

            actual.Should().BeEquivalentTo(expected.AsEnumerable());
        }

        private IRegion Alaska => _sut.Alaska;
        private IRegion Alberta => _sut.Alberta;
        private IRegion NewGuinea => _sut.NewGuinea;
        private IRegion Indonesia => _sut.Indonesia;
        private IRegion EasternAustralia => _sut.EasternAustralia;
        private IRegion Yakutsk => _sut.Yakutsk;
        private IRegion Ural => _sut.Ural;
        private IRegion Siberia => _sut.Siberia;
        private IRegion Siam => _sut.Siam;
        private IRegion Mongolia => _sut.Mongolia;
        private IRegion MiddleEast => _sut.MiddleEast;
        private IRegion Kamchatka => _sut.Kamchatka;
        private IRegion Japan => _sut.Japan;
        private IRegion Irkutsk => _sut.Irkutsk;
        private IRegion India => _sut.India;
        private IRegion China => _sut.China;
        private IRegion Afghanistan => _sut.Afghanistan;
        private IRegion SouthAfrica => _sut.SouthAfrica;
        private IRegion NorthAfrica => _sut.NorthAfrica;
        private IRegion Madagascar => _sut.Madagascar;
        private IRegion Egypt => _sut.Egypt;
        private IRegion EastAfrica => _sut.EastAfrica;
        private IRegion Congo => _sut.Congo;
        private IRegion WesternEurope => _sut.WesternEurope;
        private IRegion Ukraine => _sut.Ukraine;
        private IRegion SouthernEurope => _sut.SouthernEurope;
        private IRegion Scandinavia => _sut.Scandinavia;
        private IRegion NorthernEurope => _sut.NorthernEurope;
        private IRegion Iceland => _sut.Iceland;
        private IRegion GreatBritain => _sut.GreatBritain;
        private IRegion Venezuela => _sut.Venezuela;
        private IRegion Peru => _sut.Peru;
        private IRegion Brazil => _sut.Brazil;
        private IRegion Argentina => _sut.Argentina;
        private IRegion WesternUnitedStates => _sut.WesternUnitedStates;
        private IRegion Quebec => _sut.Quebec;
        private IRegion Ontario => _sut.Ontario;
        private IRegion Northwest => _sut.NorthwestRegion;
        private IRegion Greenland => _sut.Greenland;
        private IRegion EasternUnitedStates => _sut.EasternUnitedStates;
        private IRegion CentralAmerica => _sut.CentralAmerica;
        private IRegion WesternAustralia => _sut.WesternAustralia;
    }
}