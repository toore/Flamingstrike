using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class InteractionStateFsmTests
    {
        private readonly InteractionStateFsm _sut;

        public InteractionStateFsmTests()
        {
            _sut = new InteractionStateFsm();
        }

        [Fact]
        public void CanClick_routes_call()
        {
            var interactionState = Substitute.For<IInteractionState>();
            var region = Substitute.For<IRegion>();

            _sut.Set(interactionState);
            _sut.CanClick(region);

            interactionState.Received().CanClick(region);
        }

        [Fact]
        public void OnClick_routes_call()
        {
            var interactionState = Substitute.For<IInteractionState>();
            var region = Substitute.For<IRegion>();

            _sut.Set(interactionState);
            _sut.OnClick(region);

            interactionState.Received().OnClick(region);
        }
    }
}