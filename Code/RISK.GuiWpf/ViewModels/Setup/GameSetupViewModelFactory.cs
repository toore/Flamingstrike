using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application.GameSetup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup);
    }

    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGuiThreadDispatcher guiThreadDispatcher,
            ITaskEx taskEx)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _guiThreadDispatcher = guiThreadDispatcher;
            _taskEx = taskEx;
        }

        public IGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _dialogManager, _eventAggregator, _userInteractor, alternateGameSetup, _guiThreadDispatcher, _taskEx);
        }
    }
}