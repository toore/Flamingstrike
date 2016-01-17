using System;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public abstract class SelectStateBase : IInteractionState
    {
        public bool CanClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            var territory = stateController.Game.Territories.GetFromGeography(territoryGeography);

            return stateController.Game.IsCurrentPlayerOccupyingTerritory(territory);
        }

        public void OnClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            if (!CanClick(stateController, territoryGeography))
            {
                throw new InvalidOperationException();
            }

            stateController.SelectedTerritoryGeography = territoryGeography;
            stateController.CurrentState = CreateStateToEnterWhenSelected();
        }

        protected abstract IInteractionState CreateStateToEnterWhenSelected();
    }
}