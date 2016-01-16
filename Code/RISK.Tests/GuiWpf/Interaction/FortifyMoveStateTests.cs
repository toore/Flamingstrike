using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifyMoveStateTests : InteractionStateTestsBase
    {
        private readonly ITerritoryGeography _selectedTerritoryGeography;

        public FortifyMoveStateTests()
        {
            _selectedTerritoryGeography = Substitute.For<ITerritoryGeography>();

            _sut.CurrentState = new FortifyMoveState(_interactionStateFactory);
            _sut.SelectedTerritoryGeography = _selectedTerritoryGeography;
        }

        [Fact]
        public void Can_click_territory_that_can_be_fortified()
        {
            var territoryIdToAttack = Substitute.For<ITerritoryGeography>();
            _game.CanFortify(_selectedTerritoryGeography, territoryIdToAttack).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(territoryIdToAttack);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedTerritoryGeography);
        }

        [Fact]
        public void Can_not_click_when_fortification_is_not_allowed()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(Substitute.For<ITerritoryGeography>());
        }

        [Fact]
        public void OnClick_fortifies()
        {
            var territoryIdToFortify = Substitute.For<ITerritoryGeography>();
            _game.CanFortify(_selectedTerritoryGeography, territoryIdToFortify).Returns(true);

            _sut.OnClick(territoryIdToFortify);

            _game.Received().Fortify(_selectedTerritoryGeography, territoryIdToFortify);
        }

        [Fact]
        public void OnClick_enters_end_turn_state_after_fortification()
        {
            var endTurnState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateEndTurnState().Returns(endTurnState);
            var territoryIdToFortify = Substitute.For<ITerritoryGeography>();
            _game.CanFortify(_selectedTerritoryGeography, territoryIdToFortify).Returns(true);

            _sut.OnClick(territoryIdToFortify);

            _sut.CurrentState.Should().Be(endTurnState);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_fortify_select_state()
        {
            var fortifySelectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifySelectState().Returns(fortifySelectState);

            _sut.OnClick(_selectedTerritoryGeography);

            _sut.CurrentState.Should().Be(fortifySelectState);
        }

        [Fact]
        public void OnClick_reset_selected_territory_before_entering_fortify_select_state()
        {
            _sut.OnClick(_selectedTerritoryGeography);

            _sut.SelectedTerritoryGeography.Should().BeNull();
        }
    }
}