using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.World;

namespace RISK.Tests.GuiWpf.Interaction
{
    public static class StateControllerExtensions
    {
        public static void AssertCanClickAndOnClickCanBeInvoked(this IStateController stateController, ITerritoryGeography territoryGeography)
        {
            stateController.CanClick(territoryGeography).Should().BeTrue();

            Action act = () => stateController.OnClick(territoryGeography);
            act.ShouldNotThrow();
        }

        public static void AssertCanNotClickAndOnClickThrowsWhenInvoked(this IStateController stateController, ITerritoryGeography territoryGeography)
        {
            stateController.CanClick(territoryGeography).Should().BeFalse();

            Action act = () => stateController.OnClick(territoryGeography);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}