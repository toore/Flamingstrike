using GuiWpf.Services;
using RISK.Core;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor : ITerritoryResponder {}

    public class UserInteractor : IUserInteractor
    {
        private readonly IUserInteractionFactory _userInteractionFactory;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly IGameSetupViewModel _gameSetupViewModel;

        public UserInteractor(IUserInteractionFactory userInteractionFactory, IGuiThreadDispatcher guiThreadDispatcher, IGameSetupViewModel gameSetupViewModel)
        {
            _userInteractionFactory = userInteractionFactory;
            _guiThreadDispatcher = guiThreadDispatcher;
            _gameSetupViewModel = gameSetupViewModel;
        }

        public IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            var userInteraction = _userInteractionFactory.Create();

            _guiThreadDispatcher.Invoke(() => UpdateView(territoryRequestParameter, userInteraction));

            return userInteraction.WaitForTerritoryToBeSelected(territoryRequestParameter);
        }

        private void UpdateView(ITerritoryRequestParameter territoryRequestParameter, IUserInteraction userInteraction)
        {
            _gameSetupViewModel.UpdateView(
                territoryRequestParameter.Territories,
                userInteraction.SelectTerritory,
                territoryRequestParameter.EnabledTerritories,
                territoryRequestParameter.Player.Name,
                territoryRequestParameter.GetArmiesLeftToPlace());
        }
    }
}