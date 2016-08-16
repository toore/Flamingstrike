using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class DraftArmiesInteractionStateTests
    {
        private readonly IInteractionContext _interactionContext;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;
        private readonly DraftArmiesInteractionState _sut;

        public DraftArmiesInteractionStateTests()
        {
            _interactionContext = Substitute.For<IInteractionContext>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _game = Substitute.For<IGame>();

            _sut = new DraftArmiesInteractionState(_interactionContext, _interactionStateFactory, _game);
        }

        [Fact]
        public void Can_click_region_occupied_by_current_player_and_with_armies_to_draft()
        {
            var region = Substitute.For<IRegion>();
            _game.IsCurrentPlayerOccupyingRegion(region).Returns(true);
            _game.GetNumberOfArmiesToDraft().Returns(1);

            _sut.AssertCanClickAndOnClickCanBeInvoked(region);
        }

        [Fact]
        public void Can_not_click_region_not_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _game.GetNumberOfArmiesToDraft().Returns(1);

            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(region);
        }

        [Fact]
        public void Can_not_click_region_when_no_armies_to_draft()
        {
            var region = Substitute.For<IRegion>();
            _game.IsCurrentPlayerOccupyingRegion(region).Returns(true);

            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(region);
        }

        [Fact]
        public void OnClick_places_draft_army()
        {
            var region = Substitute.For<IRegion>();
            _game.IsCurrentPlayerOccupyingRegion(region).Returns(true);
            _game.GetNumberOfArmiesToDraft().Returns(1);

            _sut.OnClick(region);

            _game.Received().PlaceDraftArmies(region, 1);
        }

        [Fact]
        public void OnClick_enters_select_interaction_state_when_no_armies_left_to_place()
        {
            var region = Substitute.For<IRegion>();
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectInteractionState(_game).Returns(selectState);
            _game.IsCurrentPlayerOccupyingRegion(region).Returns(true);
            _game.GetNumberOfArmiesToDraft().Returns(1, 0);

            _sut.OnClick(region);

            _interactionContext.Received().Set(selectState);
        }
    }
}