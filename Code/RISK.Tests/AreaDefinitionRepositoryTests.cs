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
    public class AreaDefinitionRepositoryTests
    {
        private AreaDefinitionRepository _areaDefinitionRepository;
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

            _areaDefinitionRepository = new AreaDefinitionRepository(continentProvider);
        }

        [Test]
        public void GetAll_returns_42_areas()
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
        public void Alaskas_connected_areas_are_alberta_and_northwest_territory_and_kamchatka()
        {
            Alaska.ConnectedAreas.Should().BeEquivalentTo(Alberta, NorthwestTerritory, Kamchatka);
        }

        [Test]
        public void Alberta_is_in_north_america_and_connected_areas_are_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(_northAmerica);
            Alberta.ConnectedAreas.Should().BeEquivalentTo(Alaska, NorthwestTerritory, Ontario, WesternUnitedStates);
        }

        [Test]
        public void All_areas_has_correct_connected_areas()
        {
            AssertConnectedAreas(Alaska, Alberta, NorthwestTerritory, Kamchatka);
            AssertConnectedAreas(Alberta, Alaska, NorthwestTerritory, Ontario, WesternUnitedStates);
            AssertConnectedAreas(CentralAmerica, EasternUnitedStates, WesternUnitedStates, Venezuela);
            AssertConnectedAreas(EasternUnitedStates, CentralAmerica, Ontario, Quebec, WesternUnitedStates);
            AssertConnectedAreas(Greenland, NorthwestTerritory, Ontario, Quebec, Iceland);
            AssertConnectedAreas(NorthwestTerritory, Alaska, Alberta, Greenland, Ontario);
            AssertConnectedAreas(Ontario, Alberta, EasternUnitedStates, Greenland, NorthwestTerritory, Quebec, WesternUnitedStates);
            AssertConnectedAreas(Quebec, EasternUnitedStates, Greenland, Ontario);
            AssertConnectedAreas(WesternUnitedStates, Alberta, CentralAmerica, EasternUnitedStates, Ontario);

            AssertConnectedAreas(Argentina, Brazil, Peru);
            AssertConnectedAreas(Brazil, Argentina, Peru, Venezuela, NorthAfrica);
            AssertConnectedAreas(Peru, Argentina, Brazil, Venezuela);
            AssertConnectedAreas(Venezuela, Brazil, Peru, CentralAmerica);

            AssertConnectedAreas(GreatBritain, Iceland, NorthernEurope, Scandinavia, WesternEurope);
            AssertConnectedAreas(Iceland, GreatBritain, Scandinavia, Greenland);
            AssertConnectedAreas(NorthernEurope, GreatBritain, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
            AssertConnectedAreas(Scandinavia, GreatBritain, Iceland, NorthernEurope, Ukraine);
            AssertConnectedAreas(SouthernEurope, NorthernEurope, Ukraine, WesternEurope, Egypt, NorthAfrica, MiddleEast);
            AssertConnectedAreas(Ukraine, NorthernEurope, Scandinavia, SouthernEurope, Afghanistan, MiddleEast, Ural);
            AssertConnectedAreas(WesternEurope, GreatBritain, NorthernEurope, SouthernEurope, NorthAfrica);

            AssertConnectedAreas(Congo, EastAfrica, NorthAfrica, SouthAfrica);
            AssertConnectedAreas(EastAfrica, Congo, Egypt, Madagascar, NorthAfrica, SouthAfrica, MiddleEast);
            AssertConnectedAreas(Egypt, EastAfrica, NorthAfrica, SouthernEurope, MiddleEast);
            AssertConnectedAreas(Madagascar, EastAfrica, SouthAfrica);
            AssertConnectedAreas(NorthAfrica, Congo, EastAfrica, Egypt, Brazil, SouthernEurope, WesternEurope);
            AssertConnectedAreas(SouthAfrica, Congo, EastAfrica, Madagascar);

            AssertConnectedAreas(Afghanistan, China, India, MiddleEast, Ural, Ukraine);
            AssertConnectedAreas(China, Afghanistan, India, Mongolia, Siam, Siberia, Ural);
            AssertConnectedAreas(India, Afghanistan, China, MiddleEast, Siam);
            AssertConnectedAreas(Irkutsk, Kamchatka, Mongolia, Siberia, Yakutsk);
            AssertConnectedAreas(Japan, Kamchatka, Mongolia);
            AssertConnectedAreas(Kamchatka, Irkutsk, Japan, Mongolia, Yakutsk, Alaska);
            AssertConnectedAreas(MiddleEast, Afghanistan, India, SouthernEurope, Ukraine, EastAfrica, Egypt);
            AssertConnectedAreas(Mongolia, China, Irkutsk, Japan, Kamchatka, Siberia);
            AssertConnectedAreas(Siam, China, India, Indonesia);
            AssertConnectedAreas(Siberia,China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertConnectedAreas(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertConnectedAreas(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertConnectedAreas(EasternAustralia, NewGuinea, WesternAustralia);
            AssertConnectedAreas(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertConnectedAreas(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertConnectedAreas(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
        }

        [Test]
        public void North_america_has_9_areas()
        {
            AssertAreasInContinent(_northAmerica, 9, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, NorthwestTerritory, Ontario, Quebec, WesternUnitedStates);
        }

        [Test]
        public void South_america_has_4_areas()
        {
            AssertAreasInContinent(_southAmerica, 4, Argentina, Brazil, Peru, Venezuela);
        }

        [Test]
        public void Europe_has_7_areas()
        {
            AssertAreasInContinent(_europe, 7, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Test]
        public void Africa_has_6_areas()
        {
            AssertAreasInContinent(_africa, 6, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Test]
        public void Asia_has_12_areas()
        {
            AssertAreasInContinent(_asia, 12, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Test]
        public void Australia_has_4_areas()
        {
            AssertAreasInContinent(_australia, 4, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
        }

        [Test]
        public void GetAll_contains_all_areas()
        {
            var expectedAreas = new[]
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

            GetAll().Should().Contain(expectedAreas);
        }

        private void AssertConnectedAreas(IAreaDefinition actualAreaDefinition, params IAreaDefinition[] expectedConnectedAreas)
        {
            actualAreaDefinition.ConnectedAreas.Should().BeEquivalentTo(expectedConnectedAreas.ToList(),
                actualAreaDefinition.TranslationKey + " should have connected areas " + string.Join(", ", expectedConnectedAreas.Select(x => x.TranslationKey)));

            foreach (var expectedConnectedArea in expectedConnectedAreas)
            {
                expectedConnectedArea.ConnectedAreas.Should().Contain(actualAreaDefinition,
                    expectedConnectedArea.TranslationKey + " should recognize connected area " + actualAreaDefinition.TranslationKey +
                    " but has connected areas: " + string.Join(", ", expectedConnectedArea.ConnectedAreas.Select(x => x.TranslationKey)));
            }
        }

        private void AssertAreasInContinent(Continent continent, int expectedAreasCount, params IAreaDefinition[] expectedAreasDefinition)
        {
            var actualAreas = GetAll().Where(x => x.Continent == continent).ToList();

            actualAreas.Count.Should().Be(expectedAreasCount);
            actualAreas.Should().BeEquivalentTo(expectedAreasDefinition.ToList());
        }

        private IEnumerable<IAreaDefinition> GetAll()
        {
            return _areaDefinitionRepository.GetAll();
        }

        private IAreaDefinition Alaska
        {
            get { return _areaDefinitionRepository.Alaska; }
        }

        private IAreaDefinition Alberta
        {
            get { return _areaDefinitionRepository.Alberta; }
        }

        private IAreaDefinition NewGuinea
        {
            get { return _areaDefinitionRepository.NewGuinea; }
        }

        private IAreaDefinition Indonesia
        {
            get { return _areaDefinitionRepository.Indonesia; }
        }

        private IAreaDefinition EasternAustralia
        {
            get { return _areaDefinitionRepository.EasternAustralia; }
        }

        private IAreaDefinition Yakutsk
        {
            get { return _areaDefinitionRepository.Yakutsk; }
        }

        private IAreaDefinition Ural
        {
            get { return _areaDefinitionRepository.Ural; }
        }

        private IAreaDefinition Siberia
        {
            get { return _areaDefinitionRepository.Siberia; }
        }

        private IAreaDefinition Siam
        {
            get { return _areaDefinitionRepository.Siam; }
        }

        private IAreaDefinition Mongolia
        {
            get { return _areaDefinitionRepository.Mongolia; }
        }

        private IAreaDefinition MiddleEast
        {
            get { return _areaDefinitionRepository.MiddleEast; }
        }

        private IAreaDefinition Kamchatka
        {
            get { return _areaDefinitionRepository.Kamchatka; }
        }

        private IAreaDefinition Japan
        {
            get { return _areaDefinitionRepository.Japan; }
        }

        private IAreaDefinition Irkutsk
        {
            get { return _areaDefinitionRepository.Irkutsk; }
        }

        private IAreaDefinition India
        {
            get { return _areaDefinitionRepository.India; }
        }

        private IAreaDefinition China
        {
            get { return _areaDefinitionRepository.China; }
        }

        private IAreaDefinition Afghanistan
        {
            get { return _areaDefinitionRepository.Afghanistan; }
        }

        private IAreaDefinition SouthAfrica
        {
            get { return _areaDefinitionRepository.SouthAfrica; }
        }

        private IAreaDefinition NorthAfrica
        {
            get { return _areaDefinitionRepository.NorthAfrica; }
        }

        private IAreaDefinition Madagascar
        {
            get { return _areaDefinitionRepository.Madagascar; }
        }

        private IAreaDefinition Egypt
        {
            get { return _areaDefinitionRepository.Egypt; }
        }

        private IAreaDefinition EastAfrica
        {
            get { return _areaDefinitionRepository.EastAfrica; }
        }

        private IAreaDefinition Congo
        {
            get { return _areaDefinitionRepository.Congo; }
        }

        private IAreaDefinition WesternEurope
        {
            get { return _areaDefinitionRepository.WesternEurope; }
        }

        private IAreaDefinition Ukraine
        {
            get { return _areaDefinitionRepository.Ukraine; }
        }

        private IAreaDefinition SouthernEurope
        {
            get { return _areaDefinitionRepository.SouthernEurope; }
        }

        private IAreaDefinition Scandinavia
        {
            get { return _areaDefinitionRepository.Scandinavia; }
        }

        private IAreaDefinition NorthernEurope
        {
            get { return _areaDefinitionRepository.NorthernEurope; }
        }

        private IAreaDefinition Iceland
        {
            get { return _areaDefinitionRepository.Iceland; }
        }

        private IAreaDefinition GreatBritain
        {
            get { return _areaDefinitionRepository.GreatBritain; }
        }

        private IAreaDefinition Venezuela
        {
            get { return _areaDefinitionRepository.Venezuela; }
        }

        private IAreaDefinition Peru
        {
            get { return _areaDefinitionRepository.Peru; }
        }

        private IAreaDefinition Brazil
        {
            get { return _areaDefinitionRepository.Brazil; }
        }

        private IAreaDefinition Argentina
        {
            get { return _areaDefinitionRepository.Argentina; }
        }

        private IAreaDefinition WesternUnitedStates
        {
            get { return _areaDefinitionRepository.WesternUnitedStates; }
        }

        private IAreaDefinition Quebec
        {
            get { return _areaDefinitionRepository.Quebec; }
        }

        private IAreaDefinition Ontario
        {
            get { return _areaDefinitionRepository.Ontario; }
        }

        private IAreaDefinition NorthwestTerritory
        {
            get { return _areaDefinitionRepository.NorthwestTerritory; }
        }

        private IAreaDefinition Greenland
        {
            get { return _areaDefinitionRepository.Greenland; }
        }

        private IAreaDefinition EasternUnitedStates
        {
            get { return _areaDefinitionRepository.EasternUnitedStates; }
        }

        private IAreaDefinition CentralAmerica
        {
            get { return _areaDefinitionRepository.CentralAmerica; }
        }

        private IAreaDefinition WesternAustralia
        {
            get { return _areaDefinitionRepository.WesternAustralia; }
        }
    }
}