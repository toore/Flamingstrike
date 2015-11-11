using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.World;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class StateControllerExtensions
    {
        public static void AssertCanClickAndCanBeInvoked(this IStateController stateController, ITerritoryId territoryId)
        {
            stateController.CanClick(territoryId).Should().BeTrue();

            Action act = () => stateController.OnClick(territoryId);
            act.ShouldNotThrow<InvalidOperationException>();
        }

        public static void AssertCanNotClickAndThrowsIfInvoked(this IStateController stateController, ITerritoryId territoryId)
        {
            stateController.CanClick(territoryId).Should().BeFalse();

            Action act = () => stateController.OnClick(territoryId);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}