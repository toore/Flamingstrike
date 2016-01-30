using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifyMoveState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifyMoveState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IStateController stateController, IRegion selectedRegion)
        {
            return CanFortify(stateController, selectedRegion)
                   ||
                   CanDeselect(stateController, selectedRegion);
        }

        public void OnClick(IStateController stateController, IRegion region)
        {
            if (CanDeselect(stateController, region))
            {
                Deselect(stateController);
            }
            else if (CanFortify(stateController, region))
            {
                Fortify(stateController, region);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanDeselect(IStateController stateController, IRegion region)
        {
            return region == stateController.SelectedRegion;
        }

        private void Deselect(IStateController stateController)
        {
            stateController.CurrentState = _interactionStateFactory.CreateFortifySelectState();
            stateController.SelectedRegion = null;
        }

        private static bool CanFortify(IStateController stateController, IRegion regionToFortify)
        {
            var sourceTerritory = stateController.Game.GetTerritory(stateController.SelectedRegion);
            var territoryToFortify = stateController.Game.GetTerritory(regionToFortify);

            var canFortify = stateController.Game.CanFortify(sourceTerritory, territoryToFortify);
            return canFortify;
        }

        private void Fortify(IStateController stateController, IRegion regionToFortify)
        {
            var sourceTerritory = stateController.Game.GetTerritory(stateController.SelectedRegion);
            var territoryToFortify = stateController.Game.GetTerritory(regionToFortify);

            stateController.Game.Fortify(sourceTerritory, territoryToFortify);
            stateController.CurrentState = _interactionStateFactory.CreateEndTurnState();
        }
    }
}