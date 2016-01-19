﻿using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class SelectStateTests : InteractionStateTestsBase
    {
        public SelectStateTests()
        {
            _sut.CurrentState = new SelectState(_interactionStateFactory);
        }

        [Fact]
        public void Can_click_territory_occupied_by_current_player()
        {
            var territory = AddTerritoryOccupiedByCurrentPlayer();

            _sut.AssertCanClickAndOnClickCanBeInvoked(territory);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            var territoryGeography = Substitute.For<ITerritoryGeography>();
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(territoryGeography);
        }

        [Fact]
        public void OnClick_enters_attack_state()
        {
            var attackState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState().Returns(attackState);
            var territory = AddTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.CurrentState.Should().Be(attackState);
        }

        [Fact]
        public void OnClick_sets_selected_territory_before_entering_attack_state()
        {
            var territory = AddTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.SelectedTerritoryGeography.Should().Be(territory);
        }
    }
}