﻿using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifySelectStateTests : InteractionStateTestsBase
    {
        public FortifySelectStateTests()
        {
            _sut.CurrentState = new FortifySelectState(_interactionStateFactory);
        }

        [Fact]
        public void Can_click_territory_occupied_by_current_player()
        {
            var territoryId = CreateTerritoryOccupiedByCurrentPlayer();
            _sut.AssertCanClickAndOnClickCanBeInvoked(territoryId);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(Substitute.For<ITerritoryGeography>());
        }

        [Fact]
        public void OnClick_enters_fortify_move_state()
        {
            var fortifyMoveState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifyMoveState().Returns(fortifyMoveState);
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.CurrentState.Should().Be(fortifyMoveState);
        }

        [Fact]
        public void OnClick_sets_selected_territory_before_entering_fortify_move_state()
        {
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.SelectedTerritoryGeography.Should().Be(territory);
        }
    }
}