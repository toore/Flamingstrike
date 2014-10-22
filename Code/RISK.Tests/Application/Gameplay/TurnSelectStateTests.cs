using System;
using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnSelectStateTests : TurnStateTestsBase
    {
        private readonly ILocation _locationOwnedByPlayer;
        private readonly ITerritory _territoryOwnedByPlayer;
        private readonly ILocation _locationNotOwnedByPlayer;

        public TurnSelectStateTests()
        {
            Sut = new TurnStateFactory(StateController, BattleCalculator, CardFactory)
                .CreateSelectState(Player, WorldMap);

            _locationOwnedByPlayer = Substitute.For<ILocation>();
            _territoryOwnedByPlayer = WorldMap.HasTerritory(_locationOwnedByPlayer, Player);
            _locationNotOwnedByPlayer = Substitute.For<ILocation>();
        }

        [Fact]
        public void Can_select_location_owned_by_player()
        {
            Sut.CanSelect(_locationOwnedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_location_not_owned_by_player()
        {
            Sut.CanSelect(_locationNotOwnedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void Select_enters_attack_state()
        {
            Sut.Select(_locationOwnedByPlayer);

            StateController.CurrentState.ShouldBeEquivalentTo(
                new TurnStateStub
                {
                    IsTerritorySelected = true,
                    SelectedTerritory = _territoryOwnedByPlayer,
                    Player = Player
                });
        }

        [Fact]
        public void Territory_is_not_selected_when_not_owned_by_player()
        {
            var currentState = Substitute.For<ITurnState>();
            StateController.CurrentState = currentState;

            Sut.Select(_locationNotOwnedByPlayer);

            StateController.CurrentState.Should().Be(currentState);
            Sut.IsTerritorySelected.Should().BeFalse();
            Sut.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void Can_not_attack()
        {
            Sut.CanAttack(null).Should().BeFalse();
        }

        [Fact]
        public void Attack_is_not_supported()
        {
            Action act = () => Sut.Attack(Substitute.For<ILocation>());

            act.ShouldThrow<NotSupportedException>();
        }

        [Fact]
        public void Fortification_is_allowed()
        {
            Sut.IsFortificationAllowedInTurn().Should().BeTrue();
        }

        [Fact]
        public void Can_fortify_is_not_supported()
        {
            Action act = () => Sut.CanFortify(null);

            act.ShouldThrow<NotSupportedException>();
        }
    }
}