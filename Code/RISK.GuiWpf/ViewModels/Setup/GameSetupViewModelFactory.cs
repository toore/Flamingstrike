using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application.Play;
using RISK.Application.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup);
    }

    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IGameFactory _gameFactory;
        private readonly IGameAdapterFactory _gameAdapterFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModelFactory(
            IGameFactory gameFactory,
            IGameAdapterFactory gameAdapterFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGuiThreadDispatcher guiThreadDispatcher,
            ITaskEx taskEx)
        {
            _gameFactory = gameFactory;
            _gameAdapterFactory = gameAdapterFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _guiThreadDispatcher = guiThreadDispatcher;
            _taskEx = taskEx;
        }

        public IGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup)
        {
            return new GameSetupViewModel(
                _gameFactory,
                _gameAdapterFactory,
                _worldMapViewModelFactory, 
                _dialogManager, 
                _eventAggregator, 
                _userInteractor, 
                alternateGameSetup, 
                _guiThreadDispatcher, 
                _taskEx);
        }
    }
}