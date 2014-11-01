using System;
using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class SelectStateTests : InteractionStateTestsBase
    {
        private readonly ILocation _locationOccupiedByPlayer;
        private readonly ITerritory _territoryOccupiedByPlayer;
        private readonly ILocation _locationOccupiedByOtherPlayer;

        public SelectStateTests()
        {
            Sut = new SelectState(StateController, InteractionStateFactory, CardFactory, Player, WorldMap);

            _locationOccupiedByPlayer = Make.Location.Build();
            _territoryOccupiedByPlayer = Make.Territory
                .Location(_locationOccupiedByPlayer)
                .Occupant(Player).Build();
            WorldMap.GetTerritory(_locationOccupiedByPlayer).Returns(_territoryOccupiedByPlayer);
            _locationOccupiedByOtherPlayer = Substitute.For<ILocation>();
        }

        [Fact]
        public void Can_select_location_occupied_by_player()
        {
            Sut.CanSelect(_locationOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_location_not_occupied_by_player()
        {
            Sut.CanSelect(_locationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Select_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            InteractionStateFactory.CreateAttackState(Player, WorldMap, _territoryOccupiedByPlayer).Returns(expected);

            Sut.Select(_locationOccupiedByPlayer);

            StateController.CurrentState.Should().Be(expected);
        }

        [Fact]
        public void Territory_is_not_selected_when_not_occupied_by_player()
        {
            var currentState = Substitute.For<IInteractionState>();
            StateController.CurrentState = currentState;

            Sut.Select(_locationOccupiedByOtherPlayer);

            StateController.CurrentState.Should().Be(currentState);
            Sut.IsTerritorySelected.Should().BeFalse();
            Sut.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void Can_not_attack()
        {
            Sut.CanAttack(_locationOccupiedByPlayer).Should().BeFalse();
            Sut.CanAttack(_locationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Attack_is_not_supported()
        {
            Action act = () => Sut.Attack(Substitute.For<ILocation>());

            act.ShouldThrow<NotSupportedException>();
        }

        [Fact]
        public void Can_not_fortify()
        {
            Sut.CanFortify(_locationOccupiedByPlayer).Should().BeFalse();
            Sut.CanFortify(_locationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Fortify_is_not_supported()
        {
            Action act = () => Sut.Fortify(Substitute.For<ILocation>(), 0);

            act.ShouldThrow<NotSupportedException>();
        }
    }
}