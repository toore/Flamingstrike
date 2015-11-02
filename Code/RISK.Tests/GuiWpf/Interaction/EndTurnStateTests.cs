using System;
using FluentAssertions;
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
            throw new NotImplementedException();
        }

        [Fact]
        public void Click_on_any_territory_throws()
        {
            Action act = () => _sut.OnClick(Substitute.For<ITerritoryId>());

            act.ShouldThrow<InvalidOperationException>();
        }
    }
}