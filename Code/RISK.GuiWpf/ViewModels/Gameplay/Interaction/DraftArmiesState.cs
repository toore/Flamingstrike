using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class DraftArmiesState : IInteractionState
    {
        public bool CanClick(IStateController stateController, IRegion selectedRegion)
        {
            return stateController.Game.IsCurrentPlayerOccupyingTerritory(selectedRegion)
                   &&
                   stateController.Game.GetNumberOfArmiesToDraft() > 0;
        }

        public void OnClick(IStateController stateController, IRegion region)
        {
            var numberOfArmies = 1;
            stateController.Game.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}