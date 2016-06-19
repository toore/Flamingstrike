using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifySelectInteractionStateTests
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;
        private readonly FortifySelectInteractionState _sut;

        public FortifySelectInteractionStateTests()
        {
            _interactionStateFsm = Substitute.For<IInteractionStateFsm>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _game = Substitute.For<IGame>();

            _sut = new FortifySelectInteractionState(_interactionStateFsm, _interactionStateFactory, _game);
        }

        [Fact]
        public void Can_click_territory_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _game.IsCurrentPlayerOccupyingRegion(region).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(region);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();

            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(region);
        }

        [Fact]
        public void OnClick_enters_fortify_move_state()
        {
            var selectedRegion = Substitute.For<IRegion>();
            var fortifyMoveState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifyMoveInteractionState(_game, selectedRegion).Returns(fortifyMoveState);
            _game.IsCurrentPlayerOccupyingRegion(selectedRegion).Returns(true);

            _sut.OnClick(selectedRegion);

            _interactionStateFsm.Received().Set(fortifyMoveState);
        }
    }
}