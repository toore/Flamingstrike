using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IStateController stateController, ITerritory territory)
        {
            return stateController.Game.IsCurrentPlayerOccupyingTerritory(territory);
        }

        public void OnClick(IStateController stateController, ITerritory territory)
        {
            if (!CanClick(stateController, territory))
            {
                throw new InvalidOperationException();
            }

            stateController.SelectedTerritory = territory;
            stateController.CurrentState = _interactionStateFactory.CreateAttackState();
        }
    }
}