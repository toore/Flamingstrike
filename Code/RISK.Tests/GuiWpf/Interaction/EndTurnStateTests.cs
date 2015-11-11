using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class EndTurnStateTests : InteractionStateTestsBase
    {
        public EndTurnStateTests()
        {
            _sut.CurrentState = new EndTurnState();
        }

        [Fact]
        public void Can_not_click_any_territory()
        {
            _sut.AssertCanNotClickAndThrowsIfInvoked(Substitute.For<ITerritoryId>());
        }
    }
}