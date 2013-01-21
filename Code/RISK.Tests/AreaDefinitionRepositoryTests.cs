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
        public void Alaskas_neighbors_are_alberta_and_northwest_territory_and_kamchatka()
        {
            Alaska.Neighbors.Should().BeEquivalentTo(Alberta, NorthwestTerritory, Kamchatka);
        }

        [Test]
        public void Alberta_is_in_north_america_and_neighbors_are_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(_northAmerica);
            Alberta.Neighbors.Should().BeEquivalentTo(Alaska, NorthwestTerritory, Ontario, WesternUnitedStates);
        }

        [Test]
        public void All_areas_has_correct_neighbors()
        {
            AssertNeighbors(Alaska, Alberta, NorthwestTerritory, Kamchatka);
            AssertNeighbors(Alberta, Alaska, NorthwestTerritory, Ontario, WesternUnitedStates);
            AssertNeighbors(CentralAmerica, EasternUnitedStates, WesternUnitedStates, Venezuela);
            AssertNeighbors(EasternUnitedStates, CentralAmerica, Ontario, Quebec, WesternUnitedStates);
            AssertNeighbors(Greenland, NorthwestTerritory, Ontario, Quebec, Iceland);
            AssertNeighbors(NorthwestTerritory, Alaska, Alberta, Greenland, Ontario);
            AssertNeighbors(Ontario, Alberta, EasternUnitedStates, Greenland, NorthwestTerritory, Quebec, WesternUnitedStates);
            AssertNeighbors(Quebec, EasternUnitedStates, Greenland, Ontario);
            AssertNeighbors(WesternUnitedStates, Alberta, CentralAmerica, EasternUnitedStates, Ontario);

            AssertNeighbors(Argentina, Brazil, Peru);
            AssertNeighbors(Brazil, Argentina, Peru, Venezuela, NorthAfrica);
            AssertNeighbors(Peru, Argentina, Brazil, Venezuela);
            AssertNeighbors(Venezuela, Brazil, Peru, CentralAmerica);

            AssertNeighbors(GreatBritain, Iceland, NorthernEurope, Scandinavia, WesternEurope);
            AssertNeighbors(Iceland, GreatBritain, Scandinavia, Greenland);
            AssertNeighbors(NorthernEurope, GreatBritain, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
            AssertNeighbors(Scandinavia, GreatBritain, Iceland, NorthernEurope, Ukraine);
            AssertNeighbors(SouthernEurope, NorthernEurope, Ukraine, WesternEurope, Egypt, NorthAfrica, MiddleEast);
            AssertNeighbors(Ukraine, NorthernEurope, Scandinavia, SouthernEurope, Afghanistan, MiddleEast, Ural);
            AssertNeighbors(WesternEurope, GreatBritain, NorthernEurope, SouthernEurope, NorthAfrica);

            AssertNeighbors(Congo, EastAfrica, NorthAfrica, SouthAfrica);
            AssertNeighbors(EastAfrica, Congo, Egypt, Madagascar, NorthAfrica, SouthAfrica, MiddleEast);
            AssertNeighbors(Egypt, EastAfrica, NorthAfrica, SouthernEurope, MiddleEast);
            AssertNeighbors(Madagascar, EastAfrica, SouthAfrica);
            AssertNeighbors(NorthAfrica, Congo, EastAfrica, Egypt, Brazil, SouthernEurope, WesternEurope);
            AssertNeighbors(SouthAfrica, Congo, EastAfrica, Madagascar);

            AssertNeighbors(Afghanistan, China, India, MiddleEast, Ural, Ukraine);
            AssertNeighbors(China, Afghanistan, India, Mongolia, Siam, Siberia, Ural);
            AssertNeighbors(India, Afghanistan, China, MiddleEast, Siam);
            AssertNeighbors(Irkutsk, Kamchatka, Mongolia, Siberia, Yakutsk);
            AssertNeighbors(Japan, Kamchatka, Mongolia);
            AssertNeighbors(Kamchatka, Irkutsk, Japan, Mongolia, Yakutsk, Alaska);
            AssertNeighbors(MiddleEast, Afghanistan, India, SouthernEurope, Ukraine, EastAfrica, Egypt);
            AssertNeighbors(Mongolia, China, Irkutsk, Japan, Kamchatka, Siberia);
            AssertNeighbors(Siam, China, India, Indonesia);
            AssertNeighbors(Siberia,China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertNeighbors(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertNeighbors(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertNeighbors(EasternAustralia, NewGuinea, WesternAustralia);
            AssertNeighbors(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertNeighbors(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertNeighbors(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
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

        private void AssertNeighbors(IAreaDefinition actualAreaDefinition, params IAreaDefinition[] expectedNeighbors)
        {
            actualAreaDefinition.Neighbors.Should().BeEquivalentTo(expectedNeighbors.ToList(),
                actualAreaDefinition.TranslationKey + " should have neighbors " + string.Join(", ", expectedNeighbors.Select(x => x.TranslationKey)));

            foreach (var expectedNeighbor in expectedNeighbors)
            {
                expectedNeighbor.Neighbors.Should().Contain(actualAreaDefinition,
                    expectedNeighbor.TranslationKey + " should recognize neighbor " + actualAreaDefinition.TranslationKey +
                    " but has neighbors: " + string.Join(", ", expectedNeighbor.Neighbors.Select(x => x.TranslationKey)));
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