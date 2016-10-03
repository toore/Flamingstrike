using System;
using FluentAssertions;
using RISK.GameEngine;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;

namespace Tests.RISK.UI.WPF.Interaction
{
    public static class InteractionStateExtensions
    {
        public static void AssertOnClickCanBeInvoked(this IInteractionState interactionState, IRegion region)
        {
            Action act = () => interactionState.OnClick(region);
            act.ShouldNotThrow();
        }

        public static void AssertOnClickThrows<T>(this IInteractionState interactionState, IRegion region)
            where T : Exception
        {
            Action act = () => interactionState.OnClick(region);
            act.ShouldThrow<T>();
        }
    }
}