using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class LocationProviderTests
    {
        private LocationProvider _locationProvider;
        private readonly Continent _northAmerica = new Continent();
        private readonly Continent _southAmerica = new Continent();
        private readonly Continent _europe = new Continent();
        private readonly Continent _africa = new Continent();
        private readonly Continent _asia = new Continent();
        private readonly Continent _australia = new Continent();

        [SetUp]
        public void SetUp()
        {
            var continentProvider = MockRepository.GenerateStub<IContinentProvider>();
            continentProvider.Stub(x => x.NorthAmerica).Return(_northAmerica);
            continentProvider.Stub(x => x.SouthAmerica).Return(_southAmerica);
            continentProvider.Stub(x => x.Europe).Return(_europe);
            continentProvider.Stub(x => x.Africa).Return(_africa);
            continentProvider.Stub(x => x.Asia).Return(_asia);
            continentProvider.Stub(x => x.Australia).Return(_australia);

            _locationProvider = new LocationProvider(continentProvider);
        }

        [Test]
        public void GetAll_returns_42()
        {
            GetAll().Count().Should().Be(42);
            GetAll().Should().OnlyHaveUniqueItems();
        }

        [Test]
        public void GetAll_contains_alaska()
        {
            GetAll().Should().Contain(Alaska);
        }

        [Test]
        public void Alaska_is_in_north_america()
        {
            Alaska.Continent.Should().Be(_northAmerica);
        }

        [Test]
        public void Alaskas_connections_are_alberta_and_northwest_territory_and_kamchatka()
        {
            Alaska.Connections.Should().BeEquivalentTo(Alberta, Northwest, Kamchatka);
        }

        [Test]
        public void Alberta_is_in_north_america_and_connected_to_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(_northAmerica);
            Alberta.Connections.Should().BeEquivalentTo(Alaska, Northwest, Ontario, WesternUnitedStates);
        }

        [Test]
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
            AssertConnections(Siberia,China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertConnections(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertConnections(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertConnections(EasternAustralia, NewGuinea, WesternAustralia);
            AssertConnections(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertConnections(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertConnections(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
        }

        [Test]
        public void North_america_has_9()
        {
            AssertLocationsInContinent(_northAmerica, 9, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, Northwest, Ontario, Quebec, WesternUnitedStates);
        }

        [Test]
        public void South_america_has_4()
        {
            AssertLocationsInContinent(_southAmerica, 4, Argentina, Brazil, Peru, Venezuela);
        }

        [Test]
        public void Europe_has_7()
        {
            AssertLocationsInContinent(_europe, 7, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Test]
        public void Africa_has_6()
        {
            AssertLocationsInContinent(_africa, 6, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Test]
        public void Asia_has_12()
        {
            AssertLocationsInContinent(_asia, 12, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Test]
        public void Australia_has_4()
        {
            AssertLocationsInContinent(_australia, 4, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
        }

        [Test]
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
            return _locationProvider.GetAll();
        }

        private ILocation Alaska
        {
            get { return _locationProvider.Alaska; }
        }

        private ILocation Alberta
        {
            get { return _locationProvider.Alberta; }
        }

        private ILocation NewGuinea
        {
            get { return _locationProvider.NewGuinea; }
        }

        private ILocation Indonesia
        {
            get { return _locationProvider.Indonesia; }
        }

        private ILocation EasternAustralia
        {
            get { return _locationProvider.EasternAustralia; }
        }

        private ILocation Yakutsk
        {
            get { return _locationProvider.Yakutsk; }
        }

        private ILocation Ural
        {
            get { return _locationProvider.Ural; }
        }

        private ILocation Siberia
        {
            get { return _locationProvider.Siberia; }
        }

        private ILocation Siam
        {
            get { return _locationProvider.Siam; }
        }

        private ILocation Mongolia
        {
            get { return _locationProvider.Mongolia; }
        }

        private ILocation MiddleEast
        {
            get { return _locationProvider.MiddleEast; }
        }

        private ILocation Kamchatka
        {
            get { return _locationProvider.Kamchatka; }
        }

        private ILocation Japan
        {
            get { return _locationProvider.Japan; }
        }

        private ILocation Irkutsk
        {
            get { return _locationProvider.Irkutsk; }
        }

        private ILocation India
        {
            get { return _locationProvider.India; }
        }

        private ILocation China
        {
            get { return _locationProvider.China; }
        }

        private ILocation Afghanistan
        {
            get { return _locationProvider.Afghanistan; }
        }

        private ILocation SouthAfrica
        {
            get { return _locationProvider.SouthAfrica; }
        }

        private ILocation NorthAfrica
        {
            get { return _locationProvider.NorthAfrica; }
        }

        private ILocation Madagascar
        {
            get { return _locationProvider.Madagascar; }
        }

        private ILocation Egypt
        {
            get { return _locationProvider.Egypt; }
        }

        private ILocation EastAfrica
        {
            get { return _locationProvider.EastAfrica; }
        }

        private ILocation Congo
        {
            get { return _locationProvider.Congo; }
        }

        private ILocation WesternEurope
        {
            get { return _locationProvider.WesternEurope; }
        }

        private ILocation Ukraine
        {
            get { return _locationProvider.Ukraine; }
        }

        private ILocation SouthernEurope
        {
            get { return _locationProvider.SouthernEurope; }
        }

        private ILocation Scandinavia
        {
            get { return _locationProvider.Scandinavia; }
        }

        private ILocation NorthernEurope
        {
            get { return _locationProvider.NorthernEurope; }
        }

        private ILocation Iceland
        {
            get { return _locationProvider.Iceland; }
        }

        private ILocation GreatBritain
        {
            get { return _locationProvider.GreatBritain; }
        }

        private ILocation Venezuela
        {
            get { return _locationProvider.Venezuela; }
        }

        private ILocation Peru
        {
            get { return _locationProvider.Peru; }
        }

        private ILocation Brazil
        {
            get { return _locationProvider.Brazil; }
        }

        private ILocation Argentina
        {
            get { return _locationProvider.Argentina; }
        }

        private ILocation WesternUnitedStates
        {
            get { return _locationProvider.WesternUnitedStates; }
        }

        private ILocation Quebec
        {
            get { return _locationProvider.Quebec; }
        }

        private ILocation Ontario
        {
            get { return _locationProvider.Ontario; }
        }

        private ILocation Northwest
        {
            get { return _locationProvider.NorthwestTerritory; }
        }

        private ILocation Greenland
        {
            get { return _locationProvider.Greenland; }
        }

        private ILocation EasternUnitedStates
        {
            get { return _locationProvider.EasternUnitedStates; }
        }

        private ILocation CentralAmerica
        {
            get { return _locationProvider.CentralAmerica; }
        }

        private ILocation WesternAustralia
        {
            get { return _locationProvider.WesternAustralia; }
        }
    }
}