using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
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
            var territoryGeography = AddTerritoryOccupiedByCurrentPlayer();
            _sut.AssertCanClickAndOnClickCanBeInvoked(territoryGeography);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            var territoryGeographyToFortify = Substitute.For<IRegion>();
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(territoryGeographyToFortify);
        }

        [Fact]
        public void OnClick_enters_fortify_move_state()
        {
            var fortifyMoveState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifyMoveState().Returns(fortifyMoveState);
            var territoryGeography = AddTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territoryGeography);

            _sut.CurrentState.Should().Be(fortifyMoveState);
        }

        [Fact]
        public void OnClick_sets_selected_territory_before_entering_fortify_move_state()
        {
            var territory = AddTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.SelectedRegion.Should().Be(territory);
        }
    }
}