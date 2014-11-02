using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TerritoriesTests
    {
        private readonly Territories _sut;

        public TerritoriesTests()
        {
            _sut = new TerritoriesFactory().Create();
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
            AssertBorders(Alaska, Alberta, Northwest, Kamchatka);
        }

        [Fact]
        public void Alberta_is_in_north_america_has_border_to_alaska_and_northwest_territory_and_ontario_and_western_united_states()
        {
            Alberta.Continent.Should().Be(Continent.NorthAmerica);
            AssertBorders(Alberta, Alaska, Northwest, Ontario, WesternUnitedStates);
        }

        [Fact]
        public void All_have_correct_borders()
        {
            AssertBorders(Alaska, Alberta, Northwest, Kamchatka);
            AssertBorders(Alberta, Alaska, Northwest, Ontario, WesternUnitedStates);
            AssertBorders(CentralAmerica, EasternUnitedStates, WesternUnitedStates, Venezuela);
            AssertBorders(EasternUnitedStates, CentralAmerica, Ontario, Quebec, WesternUnitedStates);
            AssertBorders(Greenland, Northwest, Ontario, Quebec, Iceland);
            AssertBorders(Northwest, Alaska, Alberta, Greenland, Ontario);
            AssertBorders(Ontario, Alberta, EasternUnitedStates, Greenland, Northwest, Quebec, WesternUnitedStates);
            AssertBorders(Quebec, EasternUnitedStates, Greenland, Ontario);
            AssertBorders(WesternUnitedStates, Alberta, CentralAmerica, EasternUnitedStates, Ontario);

            AssertBorders(Argentina, Brazil, Peru);
            AssertBorders(Brazil, Argentina, Peru, Venezuela, NorthAfrica);
            AssertBorders(Peru, Argentina, Brazil, Venezuela);
            AssertBorders(Venezuela, Brazil, Peru, CentralAmerica);

            AssertBorders(GreatBritain, Iceland, NorthernEurope, Scandinavia, WesternEurope);
            AssertBorders(Iceland, GreatBritain, Scandinavia, Greenland);
            AssertBorders(NorthernEurope, GreatBritain, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
            AssertBorders(Scandinavia, GreatBritain, Iceland, NorthernEurope, Ukraine);
            AssertBorders(SouthernEurope, NorthernEurope, Ukraine, WesternEurope, Egypt, NorthAfrica, MiddleEast);
            AssertBorders(Ukraine, NorthernEurope, Scandinavia, SouthernEurope, Afghanistan, MiddleEast, Ural);
            AssertBorders(WesternEurope, GreatBritain, NorthernEurope, SouthernEurope, NorthAfrica);

            AssertBorders(Congo, EastAfrica, NorthAfrica, SouthAfrica);
            AssertBorders(EastAfrica, Congo, Egypt, Madagascar, NorthAfrica, SouthAfrica, MiddleEast);
            AssertBorders(Egypt, EastAfrica, NorthAfrica, SouthernEurope, MiddleEast);
            AssertBorders(Madagascar, EastAfrica, SouthAfrica);
            AssertBorders(NorthAfrica, Congo, EastAfrica, Egypt, Brazil, SouthernEurope, WesternEurope);
            AssertBorders(SouthAfrica, Congo, EastAfrica, Madagascar);

            AssertBorders(Afghanistan, China, India, MiddleEast, Ural, Ukraine);
            AssertBorders(China, Afghanistan, India, Mongolia, Siam, Siberia, Ural);
            AssertBorders(India, Afghanistan, China, MiddleEast, Siam);
            AssertBorders(Irkutsk, Kamchatka, Mongolia, Siberia, Yakutsk);
            AssertBorders(Japan, Kamchatka, Mongolia);
            AssertBorders(Kamchatka, Irkutsk, Japan, Mongolia, Yakutsk, Alaska);
            AssertBorders(MiddleEast, Afghanistan, India, SouthernEurope, Ukraine, EastAfrica, Egypt);
            AssertBorders(Mongolia, China, Irkutsk, Japan, Kamchatka, Siberia);
            AssertBorders(Siam, China, India, Indonesia);
            AssertBorders(Siberia, China, Irkutsk, Mongolia, Ural, Yakutsk);
            AssertBorders(Ural, Afghanistan, China, Siberia, Ukraine);
            AssertBorders(Yakutsk, Irkutsk, Kamchatka, Siberia);

            AssertBorders(EasternAustralia, NewGuinea, WesternAustralia);
            AssertBorders(Indonesia, NewGuinea, WesternAustralia, Siam);
            AssertBorders(NewGuinea, EasternAustralia, Indonesia, WesternAustralia);
            AssertBorders(WesternAustralia, EasternAustralia, Indonesia, NewGuinea);
        }

        [Fact]
        public void North_america_has_9()
        {
            AssertLocationsInContinent(Continent.NorthAmerica, 9, Alaska, Alberta, CentralAmerica, EasternUnitedStates, Greenland, Northwest, Ontario, Quebec, WesternUnitedStates);
        }

        [Fact]
        public void South_america_has_4()
        {
            AssertLocationsInContinent(Continent.SouthAmerica, 4, Argentina, Brazil, Peru, Venezuela);
        }

        [Fact]
        public void Europe_has_7()
        {
            AssertLocationsInContinent(Continent.Europe, 7, GreatBritain, Iceland, NorthernEurope, Scandinavia, SouthernEurope, Ukraine, WesternEurope);
        }

        [Fact]
        public void Africa_has_6()
        {
            AssertLocationsInContinent(Continent.Africa, 6, Congo, EastAfrica, Egypt, Madagascar, NorthAfrica, SouthAfrica);
        }

        [Fact]
        public void Asia_has_12()
        {
            AssertLocationsInContinent(Continent.Asia, 12, Afghanistan, China, India, Irkutsk, Japan, Kamchatka, MiddleEast, Mongolia, Siam, Siberia, Ural, Yakutsk);
        }

        [Fact]
        public void Australia_has_4()
        {
            AssertLocationsInContinent(Continent.Australia, 4, EasternAustralia, Indonesia, NewGuinea, WesternAustralia);
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

        private void AssertBorders(ITerritory actual, params ITerritory[] expectedItems)
        {
            foreach (var expected in expectedItems)
            {
                actual.IsBordering(expected).Should().BeTrue(actual.Name + " should be bordering " + expected.Name);
                expected.IsBordering(actual).Should().BeTrue(expected.Name + " should be bordering " + actual.Name);
            }
        }

        private void AssertLocationsInContinent(Continent continent, int expectedTerritoriesCount, params ITerritory[] expectedLocation)
        {
            var actual = GetAll().Where(x => x.Continent == continent).ToList();

            actual.Count.Should().Be(expectedTerritoriesCount);
            actual.Should().BeEquivalentTo(expectedLocation.ToList());
        }

        private IEnumerable<ITerritory> GetAll()
        {
            return _sut.GetAll();
        }

        private ITerritory Alaska
        {
            get { return _sut.Alaska; }
        }

        private ITerritory Alberta
        {
            get { return _sut.Alberta; }
        }

        private ITerritory NewGuinea
        {
            get { return _sut.NewGuinea; }
        }

        private ITerritory Indonesia
        {
            get { return _sut.Indonesia; }
        }

        private ITerritory EasternAustralia
        {
            get { return _sut.EasternAustralia; }
        }

        private ITerritory Yakutsk
        {
            get { return _sut.Yakutsk; }
        }

        private ITerritory Ural
        {
            get { return _sut.Ural; }
        }

        private ITerritory Siberia
        {
            get { return _sut.Siberia; }
        }

        private ITerritory Siam
        {
            get { return _sut.Siam; }
        }

        private ITerritory Mongolia
        {
            get { return _sut.Mongolia; }
        }

        private ITerritory MiddleEast
        {
            get { return _sut.MiddleEast; }
        }

        private ITerritory Kamchatka
        {
            get { return _sut.Kamchatka; }
        }

        private ITerritory Japan
        {
            get { return _sut.Japan; }
        }

        private ITerritory Irkutsk
        {
            get { return _sut.Irkutsk; }
        }

        private ITerritory India
        {
            get { return _sut.India; }
        }

        private ITerritory China
        {
            get { return _sut.China; }
        }

        private ITerritory Afghanistan
        {
            get { return _sut.Afghanistan; }
        }

        private ITerritory SouthAfrica
        {
            get { return _sut.SouthAfrica; }
        }

        private ITerritory NorthAfrica
        {
            get { return _sut.NorthAfrica; }
        }

        private ITerritory Madagascar
        {
            get { return _sut.Madagascar; }
        }

        private ITerritory Egypt
        {
            get { return _sut.Egypt; }
        }

        private ITerritory EastAfrica
        {
            get { return _sut.EastAfrica; }
        }

        private ITerritory Congo
        {
            get { return _sut.Congo; }
        }

        private ITerritory WesternEurope
        {
            get { return _sut.WesternEurope; }
        }

        private ITerritory Ukraine
        {
            get { return _sut.Ukraine; }
        }

        private ITerritory SouthernEurope
        {
            get { return _sut.SouthernEurope; }
        }

        private ITerritory Scandinavia
        {
            get { return _sut.Scandinavia; }
        }

        private ITerritory NorthernEurope
        {
            get { return _sut.NorthernEurope; }
        }

        private ITerritory Iceland
        {
            get { return _sut.Iceland; }
        }

        private ITerritory GreatBritain
        {
            get { return _sut.GreatBritain; }
        }

        private ITerritory Venezuela
        {
            get { return _sut.Venezuela; }
        }

        private ITerritory Peru
        {
            get { return _sut.Peru; }
        }

        private ITerritory Brazil
        {
            get { return _sut.Brazil; }
        }

        private ITerritory Argentina
        {
            get { return _sut.Argentina; }
        }

        private ITerritory WesternUnitedStates
        {
            get { return _sut.WesternUnitedStates; }
        }

        private ITerritory Quebec
        {
            get { return _sut.Quebec; }
        }

        private ITerritory Ontario
        {
            get { return _sut.Ontario; }
        }

        private ITerritory Northwest
        {
            get { return _sut.NorthwestTerritory; }
        }

        private ITerritory Greenland
        {
            get { return _sut.Greenland; }
        }

        private ITerritory EasternUnitedStates
        {
            get { return _sut.EasternUnitedStates; }
        }

        private ITerritory CentralAmerica
        {
            get { return _sut.CentralAmerica; }
        }

        private ITerritory WesternAustralia
        {
            get { return _sut.WesternAustralia; }
        }

        [Fact]
        public void Get_players_occupying_territories_has_no_players()
        {
            _sut.GetAllPlayersOccupyingTerritories().Count().Should().Be(0);
        }

        [Fact]
        public void Two_players_is_occupying_territories()
        {
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();
            _sut.Scandinavia.Occupant = player1;
            _sut.Congo.Occupant = player2;

            var allPlayersOccupyingTerritories = _sut.GetAllPlayersOccupyingTerritories().ToList();

            allPlayersOccupyingTerritories.Count().Should().Be(2);
            allPlayersOccupyingTerritories.Should().BeEquivalentTo(player1, player2);
        }
    }
}