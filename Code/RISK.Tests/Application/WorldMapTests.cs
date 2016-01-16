using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public class WorldMapTests
    {
        private readonly WorldMap _sut;

        public WorldMapTests()
        {
            _sut = new WorldMap();
        }

        [Fact]
        public void GetAll_returns_42()
        {
            GetAll().Count().Should().Be(42);
            GetAll().Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void GetAll_contains_alaska()
        {
            GetAll().Should().Contain(Alaska);
        }

        [Fact]
        public void Alaska_is_in_north_america()
        {
            Alaska.Continent.Should().Be(Continent.NorthAmerica);
        }

        [Fact]
        public void Alaskas_borders_alberta_and_northwest_territory_and_kamchatka()
        {
            AssertTerritoryBorders(Alaska, Alberta, Northwest, Kamchatka);
        }

        [Fact]
        public void Alberta_is_in_north_america_has_border_to_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(Continent.NorthAmerica);
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
            AssertTerritoriesInContinent(Continent.NorthAmerica, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, Northwest, Ontario, Quebec, WesternUnitedStates);
        }

        [Fact]
        public void South_america_has_4()
        {
            AssertTerritoriesInContinent(Continent.SouthAmerica, Argentina, Brazil, Peru, Venezuela);
        }

        [Fact]
        public void Europe_has_7()
        {
            AssertTerritoriesInContinent(Continent.Europe, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Fact]
        public void Africa_has_6()
        {
            AssertTerritoriesInContinent(Continent.Africa, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Fact]
        public void Asia_has_12()
        {
            AssertTerritoriesInContinent(Continent.Asia, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Fact]
        public void Australia_has_4()
        {
            AssertTerritoriesInContinent(Continent.Australia, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
        }

        [Fact]
        public void GetAll_contains_all()
        {
            IEnumerable<ITerritoryGeography> expected = new[]
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

            GetAll().Should().BeEquivalentTo(expected);
        }

        private void AssertTerritoryBorders(ITerritoryGeography actual, params ITerritoryGeography[] expectedItems)
        {
            foreach (var expected in expectedItems)
            {
                actual.HasBorder(expected).Should().BeTrue(actual + " should have border to " + expected);
                expected.HasBorder(actual).Should().BeTrue(expected + " should have border to " + actual);
            }
        }

        private void AssertTerritoriesInContinent(Continent continent, params ITerritoryGeography[] expected)
        {
            var actual = GetAll().Where(x => x.Continent == continent).ToList();

            actual.Should().BeEquivalentTo(expected.AsEnumerable());
        }

        private IEnumerable<ITerritoryGeography> GetAll()
        {
            return _sut.GetAll();
        }

        private ITerritoryGeography Alaska => _sut.Alaska;
        private ITerritoryGeography Alberta => _sut.Alberta;
        private ITerritoryGeography NewGuinea => _sut.NewGuinea;
        private ITerritoryGeography Indonesia => _sut.Indonesia;
        private ITerritoryGeography EasternAustralia => _sut.EasternAustralia;
        private ITerritoryGeography Yakutsk => _sut.Yakutsk;
        private ITerritoryGeography Ural => _sut.Ural;
        private ITerritoryGeography Siberia => _sut.Siberia;
        private ITerritoryGeography Siam => _sut.Siam;
        private ITerritoryGeography Mongolia => _sut.Mongolia;
        private ITerritoryGeography MiddleEast => _sut.MiddleEast;
        private ITerritoryGeography Kamchatka => _sut.Kamchatka;
        private ITerritoryGeography Japan => _sut.Japan;
        private ITerritoryGeography Irkutsk => _sut.Irkutsk;
        private ITerritoryGeography India => _sut.India;
        private ITerritoryGeography China => _sut.China;
        private ITerritoryGeography Afghanistan => _sut.Afghanistan;
        private ITerritoryGeography SouthAfrica => _sut.SouthAfrica;
        private ITerritoryGeography NorthAfrica => _sut.NorthAfrica;
        private ITerritoryGeography Madagascar => _sut.Madagascar;
        private ITerritoryGeography Egypt => _sut.Egypt;
        private ITerritoryGeography EastAfrica => _sut.EastAfrica;
        private ITerritoryGeography Congo => _sut.Congo;
        private ITerritoryGeography WesternEurope => _sut.WesternEurope;
        private ITerritoryGeography Ukraine => _sut.Ukraine;
        private ITerritoryGeography SouthernEurope => _sut.SouthernEurope;
        private ITerritoryGeography Scandinavia => _sut.Scandinavia;
        private ITerritoryGeography NorthernEurope => _sut.NorthernEurope;
        private ITerritoryGeography Iceland => _sut.Iceland;
        private ITerritoryGeography GreatBritain => _sut.GreatBritain;
        private ITerritoryGeography Venezuela => _sut.Venezuela;
        private ITerritoryGeography Peru => _sut.Peru;
        private ITerritoryGeography Brazil => _sut.Brazil;
        private ITerritoryGeography Argentina => _sut.Argentina;
        private ITerritoryGeography WesternUnitedStates => _sut.WesternUnitedStates;
        private ITerritoryGeography Quebec => _sut.Quebec;
        private ITerritoryGeography Ontario => _sut.Ontario;
        private ITerritoryGeography Northwest => _sut.NorthwestTerritoryGeography;
        private ITerritoryGeography Greenland => _sut.Greenland;
        private ITerritoryGeography EasternUnitedStates => _sut.EasternUnitedStates;
        private ITerritoryGeography CentralAmerica => _sut.CentralAmerica;
        private ITerritoryGeography WesternAustralia => _sut.WesternAustralia;
    }
}