using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifyMoveStateTests : InteractionStateTestsBase
    {
        private readonly ITerritoryGeography _selectedTerritoryGeography;
        private readonly ITerritory _selectedTerritory;
        private readonly ITerritory _territoryToFortify;
        private readonly ITerritoryGeography _territoryGeographyToFortify;

        public FortifyMoveStateTests()
        {
            _selectedTerritory = Substitute.For<ITerritory>();
            _selectedTerritoryGeography = _selectedTerritory.TerritoryGeography;
            _game.GetTerritory(_selectedTerritoryGeography).Returns(_selectedTerritory);

            _sut.CurrentState = new FortifyMoveState(_interactionStateFactory);
            _sut.SelectedTerritoryGeography = _selectedTerritoryGeography;

            _territoryToFortify = Substitute.For<ITerritory>();
            _territoryGeographyToFortify = Substitute.For<ITerritoryGeography>();
            _game.GetTerritory(_territoryGeographyToFortify).Returns(_territoryToFortify);
        }

        [Fact]
        public void Can_click_territory_that_can_be_fortified()
        {
            _game.CanFortify(_selectedTerritory, _territoryToFortify).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(_territoryGeographyToFortify);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedTerritoryGeography);
        }

        [Fact]
        public void Can_not_click_when_fortification_is_not_allowed()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(_territoryGeographyToFortify);
        }

        [Fact]
        public void OnClick_fortifies()
        {
            _game.CanFortify(_selectedTerritory, _territoryToFortify).Returns(true);

            _sut.OnClick(_territoryGeographyToFortify);

            _game.Received().Fortify(_selectedTerritory, _territoryToFortify);
        }

        [Fact]
        public void OnClick_enters_end_turn_state_after_fortification()
        {
            var endTurnState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateEndTurnState().Returns(endTurnState);
            _game.CanFortify(_selectedTerritory, _territoryToFortify).Returns(true);

            _sut.OnClick(_territoryGeographyToFortify);

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