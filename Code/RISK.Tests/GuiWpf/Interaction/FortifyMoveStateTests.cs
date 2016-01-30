using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifyMoveStateTests : InteractionStateTestsBase
    {
        private readonly IRegion _selectedRegion;
        private readonly ITerritory _selectedTerritory;
        private readonly ITerritory _territoryToFortify;
        private readonly IRegion _regionToFortify;

        public FortifyMoveStateTests()
        {
            _selectedTerritory = Substitute.For<ITerritory>();
            _selectedRegion = _selectedTerritory.Region;
            _game.GetTerritory(_selectedRegion).Returns(_selectedTerritory);

            _sut.CurrentState = new FortifyMoveState(_interactionStateFactory);
            _sut.SelectedRegion = _selectedRegion;

            _territoryToFortify = Substitute.For<ITerritory>();
            _regionToFortify = Substitute.For<IRegion>();
            _game.GetTerritory(_regionToFortify).Returns(_territoryToFortify);
        }

        [Fact]
        public void Can_click_territory_that_can_be_fortified()
        {
            _game.CanFortify(_selectedTerritory, _territoryToFortify).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(_regionToFortify);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedRegion);
        }

        [Fact]
        public void Can_not_click_when_fortification_is_not_allowed()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(_regionToFortify);
        }

        [Fact]
        public void OnClick_fortifies()
        {
            _game.CanFortify(_selectedTerritory, _territoryToFortify).Returns(true);

            _sut.OnClick(_regionToFortify);

            _game.Received().Fortify(_selectedTerritory, _territoryToFortify);
        }

        [Fact]
        public void OnClick_enters_end_turn_state_after_fortification()
        {
            var endTurnState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateEndTurnState().Returns(endTurnState);
            _game.CanFortify(_selectedTerritory, _territoryToFortify).Returns(true);

            _sut.OnClick(_regionToFortify);

            _sut.CurrentState.Should().Be(endTurnState);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_fortify_select_state()
        {
            var fortifySelectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifySelectState().Returns(fortifySelectState);

            _sut.OnClick(_selectedRegion);

            _sut.CurrentState.Should().Be(fortifySelectState);
        }

        [Fact]
        public void OnClick_reset_selected_territory_before_entering_fortify_select_state()
        {
            _sut.OnClick(_selectedRegion);

            _sut.SelectedRegion.Should().BeNull();
        }
    }
}