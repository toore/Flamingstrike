using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class SelectInteractionStateTests
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;
        private readonly SelectInteractionState _sut;

        public SelectInteractionStateTests()
        {
            _interactionStateFsm = Substitute.For<IInteractionStateFsm>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _game = Substitute.For<IGame>();

            _sut = new SelectInteractionState(_interactionStateFsm, _interactionStateFactory, _game);
        }

        [Fact]
        public void OnClick_enters_attack_state()
        {
            var selectedRegion = Substitute.For<IRegion>();
            var attackState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackInteractionState(_game, selectedRegion).Returns(attackState);
            _game.IsCurrentPlayerOccupyingRegion(selectedRegion).Returns(true);

            _sut.OnClick(selectedRegion);

            _interactionStateFsm.Received().Set(attackState);
        }

        [Fact]
        public void Can_click_region_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _game.IsCurrentPlayerOccupyingRegion(region).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(region);
        }

        [Fact]
        public void Can_not_click_region_not_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(region);
        }
    }
}