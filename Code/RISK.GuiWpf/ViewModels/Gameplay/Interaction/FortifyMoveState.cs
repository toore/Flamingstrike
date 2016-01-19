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

        public bool CanClick(IStateController stateController, ITerritoryGeography selectedTerritoryGeography)
        {
            return CanFortify(stateController, selectedTerritoryGeography)
                   ||
                   CanDeselect(stateController, selectedTerritoryGeography);
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

        private static bool CanFortify(IStateController stateController, ITerritoryGeography territoryGeographyToFortify)
        {
            var sourceTerritory = stateController.Game.GetTerritory(stateController.SelectedTerritoryGeography);
            var territoryToFortify = stateController.Game.GetTerritory(territoryGeographyToFortify);

            var canFortify = stateController.Game.CanFortify(sourceTerritory, territoryToFortify);
            return canFortify;
        }

        private void Fortify(IStateController stateController, ITerritoryGeography territoryGeographyToFortify)
        {
            var sourceTerritory = stateController.Game.GetTerritory(stateController.SelectedTerritoryGeography);
            var territoryToFortify = stateController.Game.GetTerritory(territoryGeographyToFortify);

            stateController.Game.Fortify(sourceTerritory, territoryToFortify);
            stateController.CurrentState = _interactionStateFactory.CreateEndTurnState();
        }
    }
}