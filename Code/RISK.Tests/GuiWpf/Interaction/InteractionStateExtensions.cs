using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Core;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class InteractionStateExtensions
    {
        public static void AssertOnClickCanBeInvoked(this IInteractionState interactionState, IRegion region)
        {
            Action act = () => interactionState.OnClick(region);
            act.ShouldNotThrow();
        }

        public static void AssertOnClickThrowsInvalidOperationException(this IInteractionState interactionState, IRegion region)
        {
            Action act = () => interactionState.OnClick(region);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}