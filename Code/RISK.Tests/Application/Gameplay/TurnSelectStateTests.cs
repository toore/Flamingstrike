using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnSelectStateTests : TurnTestsBase
    {
        private readonly ITurnState _sut;
        private readonly ILocation _locationOwnedByPlayer;
        private readonly ITerritory _territoryOwnedByPlayer;
        private readonly ILocation _locationNotOwnedByPlayer;

        public TurnSelectStateTests()
        {
            var player = Substitute.For<IPlayer>();
            var worldMap = Substitute.For<IWorldMap>();
            var battleCalculator = Substitute.For<IBattleCalculator>();
            var cardFactory = Substitute.For<ICardFactory>();

            _sut = new TurnStateFactory(battleCalculator, cardFactory).CreateSelectState(player, worldMap);

            _locationOwnedByPlayer = Substitute.For<ILocation>();
            _territoryOwnedByPlayer = StubTerritory(worldMap, _locationOwnedByPlayer, player);

            _locationNotOwnedByPlayer = Substitute.For<ILocation>();
        }

        [Fact]
        public void Can_select_location()
        {
            _sut.CanSelect(_locationOwnedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_location()
        {
            _sut.CanSelect(_locationNotOwnedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void SelectedTerritory_should_be_territory()
        {
            _sut.Select(_locationOwnedByPlayer);

            _sut.IsTerritorySelected.Should().BeTrue();
            _sut.SelectedTerritory.Should().Be(_territoryOwnedByPlayer);
        }

        [Fact]
        public void No_territory_should_be_selected()
        {
            _sut.Select(_locationNotOwnedByPlayer);

            _sut.IsTerritorySelected.Should().BeFalse();
            _sut.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void Selecting_already_selected_deselects()
        {
            _sut.Select(_locationOwnedByPlayer);
            _sut.Select(_locationOwnedByPlayer);

            _sut.IsTerritorySelected.Should().BeFalse();
            _sut.SelectedTerritory.Should().BeNull();
        }
    }
}