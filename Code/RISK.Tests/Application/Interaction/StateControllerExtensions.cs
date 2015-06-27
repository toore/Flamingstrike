using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.World;

namespace RISK.Tests.Application.Interaction
{
    public static class StateControllerExtensions
    {
        public static void AssertCanClick(this IStateController stateController, ITerritory territory)
        {
            stateController.CanClick(territory).Should().BeTrue();

            Action act = () => stateController.OnClick(territory);
            act.ShouldNotThrow<InvalidOperationException>();
        }

        public static void AssertCanNotClick(this IStateController stateController, ITerritory territory)
        {
            stateController.CanClick(territory).Should().BeFalse();

            Action act = () => stateController.OnClick(territory);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}