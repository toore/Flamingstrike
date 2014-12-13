using FluentAssertions;
using NSubstitute;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class FortifyStateTests
    {
        private readonly IInteractionState _sut;
        private readonly IPlayer _player;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly StateController _stateController;

        private readonly ITerritory _selectedTerritory;
        private readonly Territory _remoteTerritoryOccupiedByOtherPlayer;
        private readonly Territory _borderingTerritoryOccupiedByOtherPlayer;
        private readonly Territory _borderingTerritoryOccupiedByPlayer;
        private readonly Territory _remoteTerritoryOccupiedByPlayer;

        public FortifyStateTests()
        {
            _stateController = new StateController();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _player = Substitute.For<IPlayer>();

            _borderingTerritoryOccupiedByPlayer = Make.Territory
                .Occupant(_player)
                .Build();

            _remoteTerritoryOccupiedByPlayer = Make.Territory
                .Occupant(_player)
                .Build();

            _selectedTerritory = Make.Territory
                .WithBorder(_borderingTerritoryOccupiedByOtherPlayer)
                .WithBorder(_borderingTerritoryOccupiedByPlayer)
                .Occupant(_player)
                .Build();

            _sut = new FortifyState();
        }

        [Fact]
        public void Can_not_fortify()
        {
            _sut.CanClick(_selectedTerritory).Should().BeFalse();
            _sut.CanClick(_borderingTerritoryOccupiedByOtherPlayer).Should().BeFalse();
            _sut.CanClick(_remoteTerritoryOccupiedByOtherPlayer).Should().BeFalse();
            _sut.CanClick(_remoteTerritoryOccupiedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_fortify_to_bordering_territory_occupied_by_player()
        {
            _sut.CanClick(_borderingTerritoryOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Fortifies_three_army_to_bordering_territory()
        {
            _selectedTerritory.Armies = 4;
            _borderingTerritoryOccupiedByPlayer.Armies = 10;

            _sut.OnClick(_borderingTerritoryOccupiedByPlayer); //3 armies how?

            _borderingTerritoryOccupiedByPlayer.Armies.Should().Be(13);
            _selectedTerritory.Armies.Should().Be(1);
        }

        //[Fact]
        //public void Fortify_enters_fortified_state()
        //{
        //    var fortifiedState = Substitute.For<IInteractionState>();
        //    _interactionStateFactory.CreateFortifiedState(_player, _worldMap).Returns(fortifiedState);

        //    _sut.Fortify(_borderingTerritoryOccupiedByPlayer, 1);

        //    _stateController.CurrentState.Should().Be(fortifiedState);
        //}
    }
}