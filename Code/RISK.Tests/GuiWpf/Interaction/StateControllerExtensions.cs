using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.World;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class StateControllerExtensions
    {
        public static void AssertCanClickAndOnClickCanBeInvoked(this IStateController stateController, IRegion region)
        {
            stateController.CanClick(region).Should().BeTrue();

            Action act = () => stateController.OnClick(region);
            act.ShouldNotThrow();
        }

        public static void AssertCanNotClickAndOnClickThrowsWhenInvoked(this IStateController stateController, IRegion region)
        {
            stateController.CanClick(region).Should().BeFalse();

            Action act = () => stateController.OnClick(region);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}