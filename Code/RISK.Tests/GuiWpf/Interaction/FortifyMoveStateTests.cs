using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifyMoveStateTests : InteractionStateTestsBase
    {
        private readonly ITerritoryGeography _selectedTerritoryGeography;
        private readonly Territory _selectedTerritory;

        public FortifyMoveStateTests()
        {
            _selectedTerritory = AddTerritory();
            _selectedTerritoryGeography = _selectedTerritory.TerritoryGeography;

            _sut.CurrentState = new FortifyMoveState(_interactionStateFactory);
            _sut.SelectedTerritoryGeography = _selectedTerritoryGeography;
        }

        [Fact]
        public void Can_click_territory_that_can_be_fortified()
        {
            var territoryToFortify = AddTerritory();

            _game.CanFortify(_selectedTerritory, territoryToFortify).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(territoryToFortify.TerritoryGeography);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedTerritoryGeography);
        }

        [Fact]
        public void Can_not_click_when_fortification_is_not_allowed()
        {
            var territory = AddTerritory();
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(territory.TerritoryGeography);
        }

        [Fact]
        public void OnClick_fortifies()
        {
            var territoryToFortify = AddTerritory();
            _game.CanFortify(_selectedTerritory, territoryToFortify).Returns(true);

            _sut.OnClick(territoryToFortify.TerritoryGeography);

            _game.Received().Fortify(_selectedTerritory, territoryToFortify);
        }

        [Fact]
        public void OnClick_enters_end_turn_state_after_fortification()
        {
            var endTurnState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateEndTurnState().Returns(endTurnState);
            var territoryToFortify = AddTerritory();
            _game.CanFortify(_selectedTerritory, territoryToFortify).Returns(true);

            _sut.OnClick(territoryToFortify.TerritoryGeography);

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