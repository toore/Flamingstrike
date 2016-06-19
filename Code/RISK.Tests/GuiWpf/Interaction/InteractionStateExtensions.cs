using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Core;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class InteractionStateExtensions
    {
        public static void AssertCanClickAndOnClickCanBeInvoked(this IInteractionState interactionState, IRegion region)
        {
            interactionState.CanClick(region).Should().BeTrue();

            Action act = () => interactionState.OnClick(region);
            act.ShouldNotThrow();
        }

        public static void AssertCanNotClickAndOnClickThrowsInvalidOperationException(this IInteractionState interactionState, IRegion region)
        {
            interactionState.CanClick(region).Should().BeFalse();

            Action act = () => interactionState.OnClick(region);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}