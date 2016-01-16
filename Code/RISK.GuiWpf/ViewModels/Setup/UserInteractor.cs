using System.Runtime.InteropServices;
using GuiWpf.Services;
using RISK.Application.Setup;
using RISK.Application.World;

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

        public ITerritoryId ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
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