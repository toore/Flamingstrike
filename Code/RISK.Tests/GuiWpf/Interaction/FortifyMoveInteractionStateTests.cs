using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifyMoveInteractionStateTests
    {
        private readonly IRegion _selectedRegion;
        private readonly IRegion _regionToFortify;
        private readonly IInteractionContext _interactionContext;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;
        private readonly FortifyMoveInteractionState _sut;

        public FortifyMoveInteractionStateTests()
        {
            _interactionContext = Substitute.For<IInteractionContext>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _game = Substitute.For<IGame>();
            _selectedRegion = Substitute.For<IRegion>();

            _sut = new FortifyMoveInteractionState(_interactionContext, _interactionStateFactory, _game, _selectedRegion);

            _regionToFortify = Substitute.For<IRegion>();
        }

        [Fact]
        public void Selected_region_is_defined()
        {
            _sut.OnClick(_selectedRegion);

            _sut.SelectedRegion.Should().Be(_selectedRegion);
        }

        [Fact]
        public void Can_click_territory_that_can_be_fortified()
        {
            _game.CanFortify(_selectedRegion, _regionToFortify).Returns(true);

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
            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(_regionToFortify);
        }

        [Fact]
        public void OnClick_fortifies()
        {
            _game.CanFortify(_selectedRegion, _regionToFortify).Returns(true);

            _sut.OnClick(_regionToFortify);

            _game.Received().Fortify(_selectedRegion, _regionToFortify, 1);
        }

        [Fact]
        public void OnClick_enters_end_turn_state_after_fortification()
        {
            var endTurnState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateEndTurnInteractionState().Returns(endTurnState);
            _game.CanFortify(_selectedRegion, _regionToFortify).Returns(true);

            _sut.OnClick(_regionToFortify);

            _interactionContext.Received().Set(endTurnState);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_fortify_select_state()
        {
            var fortifySelectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifySelectInteractionState(_game).Returns(fortifySelectState);

            _sut.OnClick(_selectedRegion);

            _interactionContext.Received().Set(fortifySelectState);
        }
    }
}