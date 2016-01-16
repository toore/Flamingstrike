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

        public bool CanClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            return CanFortify(stateController, territoryGeography)
                   ||
                   CanDeselect(stateController, territoryGeography);
        }

        public void OnClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            if (CanDeselect(stateController, territoryGeography))
            {
                Deselect(stateController);
            }
            else if (CanFortify(stateController, territoryGeography))
            {
                Fortify(stateController, territoryGeography);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanDeselect(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            return territoryGeography == stateController.SelectedTerritoryGeography;
        }

        private void Deselect(IStateController stateController)
        {
            stateController.CurrentState = _interactionStateFactory.CreateFortifySelectState();
            stateController.SelectedTerritoryGeography = null;
        }

        private void Fortify(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            stateController.Game.Fortify(stateController.SelectedTerritoryGeography, territoryGeography);
            stateController.CurrentState = _interactionStateFactory.CreateEndTurnState();
        }

        private static bool CanFortify(IStateController stateController, ITerritoryGeography territoryGeographyToFortify)
        {
            var sourceTerritory = stateController.SelectedTerritoryGeography;
            var canFortify = stateController.Game.CanFortify(sourceTerritory, territoryGeographyToFortify);
            return canFortify;
        }
    }
}