using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.EntityProviders;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class AreaProviderTests
    {
        private AreaProvider _areaProvider;
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

            _areaProvider = new AreaProvider(continentProvider);
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

        private void AssertNeighbors(IArea actualArea, params IArea[] expectedNeighbors)
        {
            actualArea.Neighbors.Should().BeEquivalentTo(expectedNeighbors.ToList(),
                actualArea.TranslationKey + " should have neighbors " + string.Join(", ", expectedNeighbors.Select(x => x.TranslationKey)));

            foreach (var expectedNeighbor in expectedNeighbors)
            {
                expectedNeighbor.Neighbors.Should().Contain(actualArea,
                    expectedNeighbor.TranslationKey + " should recognize neighbor " + actualArea.TranslationKey +
                    " but has neighbors: " + string.Join(", ", expectedNeighbor.Neighbors.Select(x => x.TranslationKey)));
            }
        }

        private void AssertAreasInContinent(Continent continent, int expectedAreasCount, params IArea[] expectedAreas)
        {
            var actualAreas = GetAll().Where(x => x.Continent == continent).ToList();

            actualAreas.Count.Should().Be(expectedAreasCount);
            actualAreas.Should().BeEquivalentTo(expectedAreas.ToList());
        }

        private IEnumerable<IArea> GetAll()
        {
            return _areaProvider.GetAll();
        }

        private IArea Alaska
        {
            get { return _areaProvider.Alaska; }
        }

        private IArea Alberta
        {
            get { return _areaProvider.Alberta; }
        }

        private IArea NewGuinea
        {
            get { return _areaProvider.NewGuinea; }
        }

        private IArea Indonesia
        {
            get { return _areaProvider.Indonesia; }
        }

        private IArea EasternAustralia
        {
            get { return _areaProvider.EasternAustralia; }
        }

        private IArea Yakutsk
        {
            get { return _areaProvider.Yakutsk; }
        }

        private IArea Ural
        {
            get { return _areaProvider.Ural; }
        }

        private IArea Siberia
        {
            get { return _areaProvider.Siberia; }
        }

        private IArea Siam
        {
            get { return _areaProvider.Siam; }
        }

        private IArea Mongolia
        {
            get { return _areaProvider.Mongolia; }
        }

        private IArea MiddleEast
        {
            get { return _areaProvider.MiddleEast; }
        }

        private IArea Kamchatka
        {
            get { return _areaProvider.Kamchatka; }
        }

        private IArea Japan
        {
            get { return _areaProvider.Japan; }
        }

        private IArea Irkutsk
        {
            get { return _areaProvider.Irkutsk; }
        }

        private IArea India
        {
            get { return _areaProvider.India; }
        }

        private IArea China
        {
            get { return _areaProvider.China; }
        }

        private IArea Afghanistan
        {
            get { return _areaProvider.Afghanistan; }
        }

        private IArea SouthAfrica
        {
            get { return _areaProvider.SouthAfrica; }
        }

        private IArea NorthAfrica
        {
            get { return _areaProvider.NorthAfrica; }
        }

        private IArea Madagascar
        {
            get { return _areaProvider.Madagascar; }
        }

        private IArea Egypt
        {
            get { return _areaProvider.Egypt; }
        }

        private IArea EastAfrica
        {
            get { return _areaProvider.EastAfrica; }
        }

        private IArea Congo
        {
            get { return _areaProvider.Congo; }
        }

        private IArea WesternEurope
        {
            get { return _areaProvider.WesternEurope; }
        }

        private IArea Ukraine
        {
            get { return _areaProvider.Ukraine; }
        }

        private IArea SouthernEurope
        {
            get { return _areaProvider.SouthernEurope; }
        }

        private IArea Scandinavia
        {
            get { return _areaProvider.Scandinavia; }
        }

        private IArea NorthernEurope
        {
            get { return _areaProvider.NorthernEurope; }
        }

        private IArea Iceland
        {
            get { return _areaProvider.Iceland; }
        }

        private IArea GreatBritain
        {
            get { return _areaProvider.GreatBritain; }
        }

        private IArea Venezuela
        {
            get { return _areaProvider.Venezuela; }
        }

        private IArea Peru
        {
            get { return _areaProvider.Peru; }
        }

        private IArea Brazil
        {
            get { return _areaProvider.Brazil; }
        }

        private IArea Argentina
        {
            get { return _areaProvider.Argentina; }
        }

        private IArea WesternUnitedStates
        {
            get { return _areaProvider.WesternUnitedStates; }
        }

        private IArea Quebec
        {
            get { return _areaProvider.Quebec; }
        }

        private IArea Ontario
        {
            get { return _areaProvider.Ontario; }
        }

        private IArea NorthwestTerritory
        {
            get { return _areaProvider.NorthwestTerritory; }
        }

        private IArea Greenland
        {
            get { return _areaProvider.Greenland; }
        }

        private IArea EasternUnitedStates
        {
            get { return _areaProvider.EasternUnitedStates; }
        }

        private IArea CentralAmerica
        {
            get { return _areaProvider.CentralAmerica; }
        }

        private IArea WesternAustralia
        {
            get { return _areaProvider.WesternAustralia; }
        }
    }
}