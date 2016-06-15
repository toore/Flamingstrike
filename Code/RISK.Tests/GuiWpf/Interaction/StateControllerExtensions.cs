using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Core;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class StateControllerExtensions
    {
        public static void AssertCanClickAndOnClickCanBeInvoked(this IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            interactionStateFsm.CanClick(region).Should().BeTrue();

            Action act = () => interactionStateFsm.OnClick(region);
            act.ShouldNotThrow();
        }

        public static void AssertCanNotClickAndOnClickThrowsWhenInvoked(this IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            interactionStateFsm.CanClick(region).Should().BeFalse();

            Action act = () => interactionStateFsm.OnClick(region);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}