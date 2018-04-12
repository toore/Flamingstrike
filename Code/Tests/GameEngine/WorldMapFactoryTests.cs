using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FluentAssertions;
using Xunit;

namespace Tests.GameEngine
{
    public class WorldMapFactoryTests
    {
        private readonly IWorldMap _sut;

        public WorldMapFactoryTests()
        {
            _sut = new WorldMapFactory().Create();
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
            IEnumerable<Region> expected = new[]
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

        private void AssertTerritoryBorders(Region actual, params Region[] expectedItems)
        {
            foreach (var expected in expectedItems)
            {
                _sut.HasBorder(actual, expected).Should().BeTrue($"{actual} should have border to {expected}");
                _sut.HasBorder(expected, actual).Should().BeTrue($"{expected} should have border to {actual}");
            }
        }

        private void AssertTerritoriesInContinent(Continent continent, params Region[] expected)
        {
            var actual = GetAll()
                .Where(x => x.Continent == continent)
                .ToList();

            actual.Should().BeEquivalentTo(expected.AsEnumerable());
        }

        private IEnumerable<Region> GetAll()
        {
            return _sut.GetAll();
        }

        private Region Alaska => Region.Alaska;
        private Region Alberta => Region.Alberta;
        private Region NewGuinea => Region.NewGuinea;
        private Region Indonesia => Region.Indonesia;
        private Region EasternAustralia => Region.EasternAustralia;
        private Region Yakutsk => Region.Yakutsk;
        private Region Ural => Region.Ural;
        private Region Siberia => Region.Siberia;
        private Region Siam => Region.Siam;
        private Region Mongolia => Region.Mongolia;
        private Region MiddleEast => Region.MiddleEast;
        private Region Kamchatka => Region.Kamchatka;
        private Region Japan => Region.Japan;
        private Region Irkutsk => Region.Irkutsk;
        private Region India => Region.India;
        private Region China => Region.China;
        private Region Afghanistan => Region.Afghanistan;
        private Region SouthAfrica => Region.SouthAfrica;
        private Region NorthAfrica => Region.NorthAfrica;
        private Region Madagascar => Region.Madagascar;
        private Region Egypt => Region.Egypt;
        private Region EastAfrica => Region.EastAfrica;
        private Region Congo => Region.Congo;
        private Region WesternEurope => Region.WesternEurope;
        private Region Ukraine => Region.Ukraine;
        private Region SouthernEurope => Region.SouthernEurope;
        private Region Scandinavia => Region.Scandinavia;
        private Region NorthernEurope => Region.NorthernEurope;
        private Region Iceland => Region.Iceland;
        private Region GreatBritain => Region.GreatBritain;
        private Region Venezuela => Region.Venezuela;
        private Region Peru => Region.Peru;
        private Region Brazil => Region.Brazil;
        private Region Argentina => Region.Argentina;
        private Region WesternUnitedStates => Region.WesternUnitedStates;
        private Region Quebec => Region.Quebec;
        private Region Ontario => Region.Ontario;
        private Region Northwest => Region.NorthwestTerritory;
        private Region Greenland => Region.Greenland;
        private Region EasternUnitedStates => Region.EasternUnitedStates;
        private Region CentralAmerica => Region.CentralAmerica;
        private Region WesternAustralia => Region.WesternAustralia;
    }
}