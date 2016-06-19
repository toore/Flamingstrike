using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class EndTurnInteractionStateTests
    {
        private readonly IInteractionState _sut;

        public EndTurnInteractionStateTests()
        {
            _sut = new EndTurnInteractionState();
        }

        [Fact]
        public void Can_not_click_any_territory()
        {
            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(Substitute.For<IRegion>());
        }
    }
}