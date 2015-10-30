using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.World;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class StateControllerExtensions
    {
        public static void AssertCanClick(this IStateController stateController, ITerritoryId territoryId)
        {
            stateController.CanClick(territoryId).Should().BeTrue();

            Action act = () => stateController.OnClick(territoryId);
            act.ShouldNotThrow<InvalidOperationException>();
        }

        public static void AssertCanNotClick(this IStateController stateController, ITerritoryId territoryId)
        {
            stateController.CanClick(territoryId).Should().BeFalse();

            Action act = () => stateController.OnClick(territoryId);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}