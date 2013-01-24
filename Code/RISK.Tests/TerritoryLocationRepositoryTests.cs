using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class TerritoryLocationRepositoryTests
    {
        private TerritoryLocationRepository _territoryLocationRepository;
        private readonly Continent _northAmerica = new Continent();
        private readonly Continent _southAmerica = new Continent();
        private readonly Continent _europe = new Continent();
        private readonly Continent _africa = new Continent();
        private readonly Continent _asia = new Continent();
        private readonly Continent _australia = new Continent();

        [SetUp]
        public void SetUp()
        {
            var continentProvider = MockRepository.GenerateStub<IContinentRepository>();
            continentProvider.Stub(x => x.NorthAmerica).Return(_northAmerica);
            continentProvider.Stub(x => x.SouthAmerica).Return(_southAmerica);
            continentProvider.Stub(x => x.Europe).Return(_europe);
            continentProvider.Stub(x => x.Africa).Return(_africa);
            continentProvider.Stub(x => x.Asia).Return(_asia);
            continentProvider.Stub(x => x.Australia).Return(_australia);

            _territoryLocationRepository = new TerritoryLocationRepository(continentProvider);
        }

        [Test]
        public void GetAll_returns_42_territories()
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
        public void Alaskas_connected_territories_are_alberta_and_northwest_territory_and_kamchatka()
        {
            Alaska.ConnectedTerritories.Should().BeEquivalentTo(Alberta, NorthwestTerritory, Kamchatka);
        }

        [Test]
        public void Alberta_is_in_north_america_and_connected_territories_are_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(_northAmerica);
            Alberta.ConnectedTerritories.Should().BeEquivalentTo(Alaska, NorthwestTerritory, Ontario, WesternUnitedStates);
        }

        [Test]
        public void All_territories_has_correct_connected_territories()
        {
            AssertConnectedTerritories(Alaska, Alberta, NorthwestTerritory, Kamchatka);
            AssertConnectedTerritories(Alberta, Alaska, NorthwestTerritory, Ontario, WesternUnitedStates);
            AssertConnectedTerritories(CentralAmerica, EasternUnitedStates, WesternUnitedStates, Venezuela);
            AssertConnectedTerritories(EasternUnitedStates, CentralAmerica, Ontario, Quebec, WesternUnitedStates);
            AssertConnectedTerritories(Greenland, NorthwestTerritory, Ontario, Quebec, Iceland);
            AssertConnectedTerritories(NorthwestTerritory, Alaska, Alberta, Greenland, Ontario);
            AssertConnectedTerritories(Ontario, Alberta, EasternUnitedStates, Greenland, NorthwestTerritory, Quebec, WesternUnitedStates);
            AssertConnectedTerritories(Quebec, EasternUnitedStates, Greenland, Ontario);
            AssertConnectedTerritories(WesternUnitedStates, Alberta, CentralAmerica, EasternUnitedStates, Ontario);

            AssertConnectedTerritories(Argentina, Brazil, Peru);
            AssertConnectedTerritories(Brazil, Argentina, Peru, Venezuela, NorthAfrica);
            AssertConnectedTerritories(Peru, Argentina, Brazil, Venezuela);
            AssertConnectedTerritories(Venezuela, Brazil, Peru, CentralAmerica);

            AssertConnectedTerritories(GreatBritain, Iceland, NorthernEurope, Scandinavia, WesternEurope);
            AssertConnectedTerritories(Iceland, GreatBritain, Scandinavia, Greenland);
            AssertConnectedTerritories(NorthernEurope, GreatBritain, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
            AssertConnectedTerritories(Scandinavia, GreatBritain, Iceland, NorthernEurope, Ukraine);
            AssertConnectedTerritories(SouthernEurope, NorthernEurope, Ukraine, WesternEurope, Egypt, NorthAfrica, MiddleEast);
            AssertConnectedTerritories(Ukraine, NorthernEurope, Scandinavia, SouthernEurope, Afghanistan, MiddleEast, Ural);
            AssertConnectedTerritories(WesternEurope, GreatBritain, NorthernEurope, SouthernEurope, NorthAfrica);

            AssertConnectedTerritories(Congo, EastAfrica, NorthAfrica, SouthAfrica);
            AssertConnectedTerritories(EastAfrica, Congo, Egypt, Madagascar, NorthAfrica, SouthAfrica, MiddleEast);
            AssertConnectedTerritories(Egypt, EastAfrica, NorthAfrica, SouthernEurope, MiddleEast);
            AssertConnectedTerritories(Madagascar, EastAfrica, SouthAfrica);
            AssertConnectedTerritories(NorthAfrica, Congo, EastAfrica, Egypt, Brazil, SouthernEurope, WesternEurope);
            AssertConnectedTerritories(SouthAfrica, Congo, EastAfrica, Madagascar);

            AssertConnectedTerritories(Afghanistan, China, India, MiddleEast, Ural, Ukraine);
            AssertConnectedTerritories(China, Afghanistan, India, Mongolia, Siam, Siberia, Ural);
            AssertConnectedTerritories(India, Afghanistan, China, MiddleEast, Siam);
            AssertConnectedTerritories(Irkutsk, Kamchatka, Mongolia, Siberia, Yakutsk);
            AssertConnectedTerritories(Japan, Kamchatka, Mongolia);
            AssertConnectedTerritories(Kamchatka, Irkutsk, Japan, Mongolia, Yakutsk, Alaska);
            AssertConnectedTerritories(MiddleEast, Afghanistan, India, SouthernEurope, Ukraine, EastAfrica, Egypt);
            AssertConnectedTerritories(Mongolia, China, Irkutsk, Japan, Kamchatka, Siberia);
            AssertConnectedTerritories(Siam, China, India, Indonesia);
            AssertConnectedTerritories(Siberia,China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertConnectedTerritories(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertConnectedTerritories(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertConnectedTerritories(EasternAustralia, NewGuinea, WesternAustralia);
            AssertConnectedTerritories(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertConnectedTerritories(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertConnectedTerritories(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
        }

        [Test]
        public void North_america_has_9_territories()
        {
            AssertTerritoriesInContinent(_northAmerica, 9, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, NorthwestTerritory, Ontario, Quebec, WesternUnitedStates);
        }

        [Test]
        public void South_america_has_4_territories()
        {
            AssertTerritoriesInContinent(_southAmerica, 4, Argentina, Brazil, Peru, Venezuela);
        }

        [Test]
        public void Europe_has_7_territories()
        {
            AssertTerritoriesInContinent(_europe, 7, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Test]
        public void Africa_has_6_territories()
        {
            AssertTerritoriesInContinent(_africa, 6, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Test]
        public void Asia_has_12_territories()
        {
            AssertTerritoriesInContinent(_asia, 12, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Test]
        public void Australia_has_4_territories()
        {
            AssertTerritoriesInContinent(_australia, 4, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
        }

        [Test]
        public void GetAll_contains_all_territories()
        {
            var expectedTerritories = new[]
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

            GetAll().Should().Contain(expectedTerritories);
        }

        private void AssertConnectedTerritories(ITerritoryLocation actualTerritoryLocation, params ITerritoryLocation[] expectedConnectedTerritories)
        {
            actualTerritoryLocation.ConnectedTerritories.Should().BeEquivalentTo(expectedConnectedTerritories.ToList(),
                actualTerritoryLocation.TranslationKey + " should have connected territories " + string.Join(", ", expectedConnectedTerritories.Select(x => x.TranslationKey)));

            foreach (var expectedConnectedArea in expectedConnectedTerritories)
            {
                expectedConnectedArea.ConnectedTerritories.Should().Contain(actualTerritoryLocation,
                    expectedConnectedArea.TranslationKey + " should recognize connected area " + actualTerritoryLocation.TranslationKey +
                    " but has connected territories: " + string.Join(", ", expectedConnectedArea.ConnectedTerritories.Select(x => x.TranslationKey)));
            }
        }

        private void AssertTerritoriesInContinent(Continent continent, int expectedTerritoriesCount, params ITerritoryLocation[] expectedTerritoriesLocation)
        {
            var actual = GetAll().Where(x => x.Continent == continent).ToList();

            actual.Count.Should().Be(expectedTerritoriesCount);
            actual.Should().BeEquivalentTo(expectedTerritoriesLocation.ToList());
        }

        private IEnumerable<ITerritoryLocation> GetAll()
        {
            return _territoryLocationRepository.GetAll();
        }

        private ITerritoryLocation Alaska
        {
            get { return _territoryLocationRepository.Alaska; }
        }

        private ITerritoryLocation Alberta
        {
            get { return _territoryLocationRepository.Alberta; }
        }

        private ITerritoryLocation NewGuinea
        {
            get { return _territoryLocationRepository.NewGuinea; }
        }

        private ITerritoryLocation Indonesia
        {
            get { return _territoryLocationRepository.Indonesia; }
        }

        private ITerritoryLocation EasternAustralia
        {
            get { return _territoryLocationRepository.EasternAustralia; }
        }

        private ITerritoryLocation Yakutsk
        {
            get { return _territoryLocationRepository.Yakutsk; }
        }

        private ITerritoryLocation Ural
        {
            get { return _territoryLocationRepository.Ural; }
        }

        private ITerritoryLocation Siberia
        {
            get { return _territoryLocationRepository.Siberia; }
        }

        private ITerritoryLocation Siam
        {
            get { return _territoryLocationRepository.Siam; }
        }

        private ITerritoryLocation Mongolia
        {
            get { return _territoryLocationRepository.Mongolia; }
        }

        private ITerritoryLocation MiddleEast
        {
            get { return _territoryLocationRepository.MiddleEast; }
        }

        private ITerritoryLocation Kamchatka
        {
            get { return _territoryLocationRepository.Kamchatka; }
        }

        private ITerritoryLocation Japan
        {
            get { return _territoryLocationRepository.Japan; }
        }

        private ITerritoryLocation Irkutsk
        {
            get { return _territoryLocationRepository.Irkutsk; }
        }

        private ITerritoryLocation India
        {
            get { return _territoryLocationRepository.India; }
        }

        private ITerritoryLocation China
        {
            get { return _territoryLocationRepository.China; }
        }

        private ITerritoryLocation Afghanistan
        {
            get { return _territoryLocationRepository.Afghanistan; }
        }

        private ITerritoryLocation SouthAfrica
        {
            get { return _territoryLocationRepository.SouthAfrica; }
        }

        private ITerritoryLocation NorthAfrica
        {
            get { return _territoryLocationRepository.NorthAfrica; }
        }

        private ITerritoryLocation Madagascar
        {
            get { return _territoryLocationRepository.Madagascar; }
        }

        private ITerritoryLocation Egypt
        {
            get { return _territoryLocationRepository.Egypt; }
        }

        private ITerritoryLocation EastAfrica
        {
            get { return _territoryLocationRepository.EastAfrica; }
        }

        private ITerritoryLocation Congo
        {
            get { return _territoryLocationRepository.Congo; }
        }

        private ITerritoryLocation WesternEurope
        {
            get { return _territoryLocationRepository.WesternEurope; }
        }

        private ITerritoryLocation Ukraine
        {
            get { return _territoryLocationRepository.Ukraine; }
        }

        private ITerritoryLocation SouthernEurope
        {
            get { return _territoryLocationRepository.SouthernEurope; }
        }

        private ITerritoryLocation Scandinavia
        {
            get { return _territoryLocationRepository.Scandinavia; }
        }

        private ITerritoryLocation NorthernEurope
        {
            get { return _territoryLocationRepository.NorthernEurope; }
        }

        private ITerritoryLocation Iceland
        {
            get { return _territoryLocationRepository.Iceland; }
        }

        private ITerritoryLocation GreatBritain
        {
            get { return _territoryLocationRepository.GreatBritain; }
        }

        private ITerritoryLocation Venezuela
        {
            get { return _territoryLocationRepository.Venezuela; }
        }

        private ITerritoryLocation Peru
        {
            get { return _territoryLocationRepository.Peru; }
        }

        private ITerritoryLocation Brazil
        {
            get { return _territoryLocationRepository.Brazil; }
        }

        private ITerritoryLocation Argentina
        {
            get { return _territoryLocationRepository.Argentina; }
        }

        private ITerritoryLocation WesternUnitedStates
        {
            get { return _territoryLocationRepository.WesternUnitedStates; }
        }

        private ITerritoryLocation Quebec
        {
            get { return _territoryLocationRepository.Quebec; }
        }

        private ITerritoryLocation Ontario
        {
            get { return _territoryLocationRepository.Ontario; }
        }

        private ITerritoryLocation NorthwestTerritory
        {
            get { return _territoryLocationRepository.NorthwestTerritory; }
        }

        private ITerritoryLocation Greenland
        {
            get { return _territoryLocationRepository.Greenland; }
        }

        private ITerritoryLocation EasternUnitedStates
        {
            get { return _territoryLocationRepository.EasternUnitedStates; }
        }

        private ITerritoryLocation CentralAmerica
        {
            get { return _territoryLocationRepository.CentralAmerica; }
        }

        private ITerritoryLocation WesternAustralia
        {
            get { return _territoryLocationRepository.WesternAustralia; }
        }
    }
}