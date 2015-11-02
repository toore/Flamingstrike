using System;
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
        public void Can_click_territory_occupied_by_current_player()
        {
            _sut.AssertCanClick(Substitute.For<ITerritoryId>());
        }

        [Fact]
        public void Click_on_territory_occupied_by_current_player_enters_fortify_move_state_and_selects_territory()
        {
            _sut.OnClick(null);

            throw new NotImplementedException();
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritoryId>());
        }

        [Fact]
        public void Click_on_any_territory_not_occupied_by_current_player_throws()
        {
            throw new NotImplementedException();
        }
    }
}