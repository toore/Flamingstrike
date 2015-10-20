using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifySelectStateTests : InteractionStateTestsBase
    {
        public FortifySelectStateTests()
        {
            _sut.CurrentState = new FortifySelectState(_interactionStateFactory);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritory>());
        }
    }
}