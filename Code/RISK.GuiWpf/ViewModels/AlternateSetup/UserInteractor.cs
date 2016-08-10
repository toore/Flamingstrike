using GuiWpf.Services;
using RISK.Core;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.AlternateSetup
{
    public interface IUserInteractor : ITerritoryResponder {}

    public class UserInteractor : IUserInteractor
    {
        private readonly IUserInteractionFactory _userInteractionFactory;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly IAlternateGameSetupViewModel _alternateGameSetupViewModel;

        public UserInteractor(IUserInteractionFactory userInteractionFactory, IGuiThreadDispatcher guiThreadDispatcher, IAlternateGameSetupViewModel alternateGameSetupViewModel)
        {
            _userInteractionFactory = userInteractionFactory;
            _guiThreadDispatcher = guiThreadDispatcher;
            _alternateGameSetupViewModel = alternateGameSetupViewModel;
        }

        public IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            var userInteraction = _userInteractionFactory.Create();

            _guiThreadDispatcher.Invoke(() => UpdateView(territoryRequestParameter, userInteraction));

            return userInteraction.WaitForTerritoryToBeSelected(territoryRequestParameter);
        }

        private void UpdateView(ITerritoryRequestParameter territoryRequestParameter, IUserInteraction userInteraction)
        {
            _alternateGameSetupViewModel.UpdateView(
                territoryRequestParameter.Territories,
                userInteraction.SelectTerritory,
                territoryRequestParameter.EnabledTerritories,
                territoryRequestParameter.Player.Name,
                territoryRequestParameter.GetArmiesLeftToPlace());
        }
    }
}