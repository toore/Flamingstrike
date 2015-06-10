using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class AttackStateTests
    {
        private readonly AttackState _sut;
        private readonly IPlayer _player;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly StateController _stateController;
        private readonly IBattleCalculator _battleCalculator;

        private readonly ITerritory _selectedTerritory;
        private readonly Territory _remoteTerritoryOccupiedByOtherPlayer;
        private readonly Territory _borderingTerritoryOccupiedByOtherPlayer;
        private readonly Territory _borderingTerritoryOccupiedByPlayer;

        public AttackStateTests()
        {
            _stateController = new StateController(_interactionStateFactory, _player, null);
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _player = Substitute.For<IPlayer>();
            _battleCalculator = Substitute.For<IBattleCalculator>();

            _borderingTerritoryOccupiedByPlayer = Make.Territory
                .Occupant(_player)
                .Build();

            var otherPlayer = Substitute.For<IPlayer>();

            _borderingTerritoryOccupiedByOtherPlayer = Make.Territory
                .Occupant(otherPlayer)
                .Build();

            _remoteTerritoryOccupiedByOtherPlayer = Make.Territory
                .Occupant(otherPlayer)
                .Build();

            _selectedTerritory = Make.Territory
                .WithBorder(_borderingTerritoryOccupiedByOtherPlayer)
                .WithBorder(_borderingTerritoryOccupiedByPlayer)
                .Occupant(_player)
                .Build();

            _sut = new AttackState(_stateController, _interactionStateFactory, _player, _selectedTerritory);
        }

        [Fact]
        public void Can_click_on_location_already_selected()
        {
            _sut.CanClick(_selectedTerritory).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_not_selected_location()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritory>());
            _sut.AssertCanNotClick(_borderingTerritoryOccupiedByOtherPlayer);
        }

        [Fact]
        public void Selecting_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState(_stateController, _player).Returns(selectState);

            _sut.OnClick(_selectedTerritory);

            _stateController.CurrentState.Should().Be(selectState);
        }

        [Fact]
        public void Can_not_click_bordering_location_when_having_one_army()
        {
            _selectedTerritory.Armies = 1;

            _sut.AssertCanNotClick(_borderingTerritoryOccupiedByOtherPlayer);
        }

        [Fact]
        public void Can_not_attack_non_bordering_location()
        {
            _selectedTerritory.Armies = 2;

            _sut.AssertCanNotClick(_remoteTerritoryOccupiedByOtherPlayer);
        }

        [Fact]
        public void Can_not_attack_bordering_location_already_occupied()
        {
            _selectedTerritory.Armies = 2;

            _sut.AssertCanNotClick(_borderingTerritoryOccupiedByPlayer);
        }

        [Fact]
        public void Can_attack_bordering_location_when_having_two_armies()
        {
            _selectedTerritory.Armies = 2;

            _sut.CanClick(_borderingTerritoryOccupiedByOtherPlayer).Should().BeTrue();
        }

        [Fact]
        public void Attacks_when_territories_have_borders()
        {
            _selectedTerritory.Armies = 2;

            _sut.OnClick(_borderingTerritoryOccupiedByOtherPlayer);

            _battleCalculator.Received().Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer);
        }

        [Fact]
        public void After_successfull_attack_attack_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState(_stateController, _player, _borderingTerritoryOccupiedByOtherPlayer).Returns(expected);
            _selectedTerritory.Armies = 2;
            AttackerWins();

            _sut.OnClick(_borderingTerritoryOccupiedByOtherPlayer);

            _stateController.CurrentState.Should().Be(expected);
        }

        private void AttackerWins()
        {
            _battleCalculator
                .When(x => x.Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer))
                .Do(x => _borderingTerritoryOccupiedByOtherPlayer.Occupant = _selectedTerritory.Occupant);
        }

        [Fact]
        public void Player_will_be_awarded_a_card_when_turn_ends()
        {
            _selectedTerritory.Armies = 2;
            _battleCalculator
                .When(x => x.Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer))
                .Do(x => _borderingTerritoryOccupiedByOtherPlayer.Occupant = _selectedTerritory.Occupant);

            _sut.OnClick(_borderingTerritoryOccupiedByOtherPlayer);

            //_stateController.PlayerShouldReceiveCardWhenTurnEnds.Should().BeTrue();
            throw new NotImplementedException();
        }
    }

    public static class GameStateAssertionExtensions
    {
        public static void AssertCanNotClick(this IInteractionState state, ITerritory territory)
        {
            state.CanClick(territory).Should().BeFalse();

            Action act = () => state.OnClick(territory);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}