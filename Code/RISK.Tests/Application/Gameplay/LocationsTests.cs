using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using RISK.Domain;
using RISK.Domain.Entities;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class LocationsTests
    {
        private Locations _locations;

        public LocationsTests()
        {
            _locations = new Locations();
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
            Alaska.Continent.Should().Be(Continents.NorthAmerica);
        }

        [Fact]
        public void Alaskas_connections_are_alberta_and_northwest_territory_and_kamchatka()
        {
            Alaska.Connections.Should().BeEquivalentTo(Alberta, Northwest, Kamchatka);
        }

        [Fact]
        public void Alberta_is_in_north_america_and_connected_to_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(Continents.NorthAmerica);
            Alberta.Connections.Should().BeEquivalentTo(Alaska, Northwest, Ontario, WesternUnitedStates);
        }

        [Fact]
        public void All_have_correct_connections()
        {
            AssertConnections(Alaska, Alberta, Northwest, Kamchatka);
            AssertConnections(Alberta, Alaska, Northwest, Ontario, WesternUnitedStates);
            AssertConnections(CentralAmerica, EasternUnitedStates, WesternUnitedStates, Venezuela);
            AssertConnections(EasternUnitedStates, CentralAmerica, Ontario, Quebec, WesternUnitedStates);
            AssertConnections(Greenland, Northwest, Ontario, Quebec, Iceland);
            AssertConnections(Northwest, Alaska, Alberta, Greenland, Ontario);
            AssertConnections(Ontario, Alberta, EasternUnitedStates, Greenland, Northwest, Quebec, WesternUnitedStates);
            AssertConnections(Quebec, EasternUnitedStates, Greenland, Ontario);
            AssertConnections(WesternUnitedStates, Alberta, CentralAmerica, EasternUnitedStates, Ontario);

            AssertConnections(Argentina, Brazil, Peru);
            AssertConnections(Brazil, Argentina, Peru, Venezuela, NorthAfrica);
            AssertConnections(Peru, Argentina, Brazil, Venezuela);
            AssertConnections(Venezuela, Brazil, Peru, CentralAmerica);

            AssertConnections(GreatBritain, Iceland, NorthernEurope, Scandinavia, WesternEurope);
            AssertConnections(Iceland, GreatBritain, Scandinavia, Greenland);
            AssertConnections(NorthernEurope, GreatBritain, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
            AssertConnections(Scandinavia, GreatBritain, Iceland, NorthernEurope, Ukraine);
            AssertConnections(SouthernEurope, NorthernEurope, Ukraine, WesternEurope, Egypt, NorthAfrica, MiddleEast);
            AssertConnections(Ukraine, NorthernEurope, Scandinavia, SouthernEurope, Afghanistan, MiddleEast, Ural);
            AssertConnections(WesternEurope, GreatBritain, NorthernEurope, SouthernEurope, NorthAfrica);

            AssertConnections(Congo, EastAfrica, NorthAfrica, SouthAfrica);
            AssertConnections(EastAfrica, Congo, Egypt, Madagascar, NorthAfrica, SouthAfrica, MiddleEast);
            AssertConnections(Egypt, EastAfrica, NorthAfrica, SouthernEurope, MiddleEast);
            AssertConnections(Madagascar, EastAfrica, SouthAfrica);
            AssertConnections(NorthAfrica, Congo, EastAfrica, Egypt, Brazil, SouthernEurope, WesternEurope);
            AssertConnections(SouthAfrica, Congo, EastAfrica, Madagascar);

            AssertConnections(Afghanistan, China, India, MiddleEast, Ural, Ukraine);
            AssertConnections(China, Afghanistan, India, Mongolia, Siam, Siberia, Ural);
            AssertConnections(India, Afghanistan, China, MiddleEast, Siam);
            AssertConnections(Irkutsk, Kamchatka, Mongolia, Siberia, Yakutsk);
            AssertConnections(Japan, Kamchatka, Mongolia);
            AssertConnections(Kamchatka, Irkutsk, Japan, Mongolia, Yakutsk, Alaska);
            AssertConnections(MiddleEast, Afghanistan, India, SouthernEurope, Ukraine, EastAfrica, Egypt);
            AssertConnections(Mongolia, China, Irkutsk, Japan, Kamchatka, Siberia);
            AssertConnections(Siam, China, India, Indonesia);
            AssertConnections(Siberia, China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertConnections(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertConnections(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertConnections(EasternAustralia, NewGuinea, WesternAustralia);
            AssertConnections(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertConnections(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertConnections(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
        }

        [Fact]
        public void North_america_has_9()
        {
            AssertLocationsInContinent(Continents.NorthAmerica, 9, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, Northwest, Ontario, Quebec, WesternUnitedStates);
        }

        [Fact]
        public void South_america_has_4()
        {
            AssertLocationsInContinent(Continents.SouthAmerica, 4, Argentina, Brazil, Peru, Venezuela);
        }

        [Fact]
        public void Europe_has_7()
        {
            AssertLocationsInContinent(Continents.Europe, 7, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Fact]
        public void Africa_has_6()
        {
            AssertLocationsInContinent(Continents.Africa, 6, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Fact]
        public void Asia_has_12()
        {
            AssertLocationsInContinent(Continents.Asia, 12, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Fact]
        public void Australia_has_4()
        {
            AssertLocationsInContinent(Continents.Australia, 4, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
        }

        [Fact]
        public void GetAll_contains_all()
        {
            var expected = new[]
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

            GetAll().Should().Contain(expected);
        }

        private void AssertConnections(ILocation actual, params ILocation[] expectedItems)
        {
            actual.Connections.Should().BeEquivalentTo(expectedItems.ToList(),
                actual.TranslationKey + " should have connected territories " + string.Join(", ", expectedItems.Select(x => x.TranslationKey)));

            foreach (var expected in expectedItems)
            {
                expected.Connections.Should().Contain(actual,
                    expected.TranslationKey + " should recognize connection " + actual.TranslationKey +
                    " but has connections: " + string.Join(", ", expected.Connections.Select(x => x.TranslationKey)));
            }
        }

        private void AssertLocationsInContinent(Continent continent, int expectedTerritoriesCount, params ILocation[] expectedLocation)
        {
            var actual = GetAll().Where(x => x.Continent == continent).ToList();

            actual.Count.Should().Be(expectedTerritoriesCount);
            actual.Should().BeEquivalentTo(expectedLocation.ToList());
        }

        private IEnumerable<ILocation> GetAll()
        {
            return _locations.GetAll();
        }

        private ILocation Alaska
        {
            get { return _locations.Alaska; }
        }

        private ILocation Alberta
        {
            get { return _locations.Alberta; }
        }

        private ILocation NewGuinea
        {
            get { return _locations.NewGuinea; }
        }

        private ILocation Indonesia
        {
            get { return _locations.Indonesia; }
        }

        private ILocation EasternAustralia
        {
            get { return _locations.EasternAustralia; }
        }

        private ILocation Yakutsk
        {
            get { return _locations.Yakutsk; }
        }

        private ILocation Ural
        {
            get { return _locations.Ural; }
        }

        private ILocation Siberia
        {
            get { return _locations.Siberia; }
        }

        private ILocation Siam
        {
            get { return _locations.Siam; }
        }

        private ILocation Mongolia
        {
            get { return _locations.Mongolia; }
        }

        private ILocation MiddleEast
        {
            get { return _locations.MiddleEast; }
        }

        private ILocation Kamchatka
        {
            get { return _locations.Kamchatka; }
        }

        private ILocation Japan
        {
            get { return _locations.Japan; }
        }

        private ILocation Irkutsk
        {
            get { return _locations.Irkutsk; }
        }

        private ILocation India
        {
            get { return _locations.India; }
        }

        private ILocation China
        {
            get { return _locations.China; }
        }

        private ILocation Afghanistan
        {
            get { return _locations.Afghanistan; }
        }

        private ILocation SouthAfrica
        {
            get { return _locations.SouthAfrica; }
        }

        private ILocation NorthAfrica
        {
            get { return _locations.NorthAfrica; }
        }

        private ILocation Madagascar
        {
            get { return _locations.Madagascar; }
        }

        private ILocation Egypt
        {
            get { return _locations.Egypt; }
        }

        private ILocation EastAfrica
        {
            get { return _locations.EastAfrica; }
        }

        private ILocation Congo
        {
            get { return _locations.Congo; }
        }

        private ILocation WesternEurope
        {
            get { return _locations.WesternEurope; }
        }

        private ILocation Ukraine
        {
            get { return _locations.Ukraine; }
        }

        private ILocation SouthernEurope
        {
            get { return _locations.SouthernEurope; }
        }

        private ILocation Scandinavia
        {
            get { return _locations.Scandinavia; }
        }

        private ILocation NorthernEurope
        {
            get { return _locations.NorthernEurope; }
        }

        private ILocation Iceland
        {
            get { return _locations.Iceland; }
        }

        private ILocation GreatBritain
        {
            get { return _locations.GreatBritain; }
        }

        private ILocation Venezuela
        {
            get { return _locations.Venezuela; }
        }

        private ILocation Peru
        {
            get { return _locations.Peru; }
        }

        private ILocation Brazil
        {
            get { return _locations.Brazil; }
        }

        private ILocation Argentina
        {
            get { return _locations.Argentina; }
        }

        private ILocation WesternUnitedStates
        {
            get { return _locations.WesternUnitedStates; }
        }

        private ILocation Quebec
        {
            get { return _locations.Quebec; }
        }

        private ILocation Ontario
        {
            get { return _locations.Ontario; }
        }

        private ILocation Northwest
        {
            get { return _locations.NorthwestTerritory; }
        }

        private ILocation Greenland
        {
            get { return _locations.Greenland; }
        }

        private ILocation EasternUnitedStates
        {
            get { return _locations.EasternUnitedStates; }
        }

        private ILocation CentralAmerica
        {
            get { return _locations.CentralAmerica; }
        }

        private ILocation WesternAustralia
        {
            get { return _locations.WesternAustralia; }
        }
    }
}