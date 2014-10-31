using System;
using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnAttackStateTests : TurnStateTestsBase
    {
        public TurnAttackStateTests()
        {
            Sut = new TurnStateFactory(StateController, BattleCalculator, CardFactory)
                .CreateAttackState(Player, WorldMap, SelectedTerritory);

            OtherLocation = Substitute.For<ILocation>();
            var otherPlayer = Substitute.For<IPlayer>();
            OtherTerritory = WorldMap.HasTerritory(OtherLocation, otherPlayer);
        }

        [Fact]
        public void Can_select_location_selected()
        {
            Sut.CanSelect(SelectedLocation).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_not_selected_location()
        {
            Sut.CanSelect(Substitute.For<ILocation>()).Should().BeFalse();
        }

        [Fact]
        public void Selecting_selected_location_enters_select_state()
        {
            Sut.Select(SelectedLocation);

            StateController.CurrentState.ShouldBeEquivalentTo(
                new TurnStateStub
                {
                    IsTerritorySelected = false,
                    SelectedTerritory = null,
                    Player = Player
                });
        }

        [Fact]
        public void Can_attack_when_territories_have_borders()
        {
            SelectedLocation.IsBordering(_otherLocation);
            SelectedTerritory.Armies = 2;

            Sut.CanAttack(OtherLocation).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack_when_having_only_one_army()
        {
            SelectedLocation.IsBordering(_otherLocation);
            SelectedTerritory.Armies = 1;

            Sut.CanAttack(OtherLocation).Should().BeFalse("there is only one army in location");
        }

        [Fact]
        public void Can_not_attack_any_location()
        {
            var canAttack = Sut.CanAttack(Substitute.For<ILocation>());

            canAttack.Should().BeFalse();
            BattleCalculator.DidNotReceiveWithAnyArgs().Attack(null, null);
        }

        [Fact]
        public void Attacks_when_territories_have_borders()
        {
            SelectedLocation.IsBordering(_otherLocation);
            SelectedTerritory.Armies = 2;

            Sut.Attack(OtherLocation);

            BattleCalculator.Received().Attack(SelectedTerritory, OtherTerritory);
        }

        [Fact]
        public void After_successfull_attack_attack_enters_attack_state()
        {
            SelectedLocation.IsConnectedTo(OtherLocation);
            SelectedTerritory.Armies = 2;
            BattleCalculator.AttackerAlwaysWins(SelectedTerritory, OtherTerritory);

            Sut.Attack(OtherLocation);

            StateController.CurrentState.Should().NotBe(Sut);
            StateController.CurrentState.ShouldBeEquivalentTo(
               new TurnStateStub
               {
                   IsTerritorySelected = true,
                   SelectedTerritory = OtherTerritory,
                   Player = Player
               });
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

        [Fact]
        public void Player_will_be_awarded_a_card_when_turn_ends()
        {
            SelectedLocation.IsBordering(_otherLocation);
            SelectedTerritory.Armies = 2;
            BattleCalculator.AttackerAlwaysWins(SelectedTerritory, OtherTerritory);

            Sut.Attack(OtherLocation);
            Sut.EndTurn();

            StateController.PlayerShouldReceiveCardWhenTurnEnds.Should().BeTrue();
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            StateController.PlayerShouldReceiveCardWhenTurnEnds = false;

            Sut.EndTurn();

            Player.DidNotReceive().AddCard(null);
        }
    }
}