using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup);
    }

    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IGameFactory _gameFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModelFactory(
            IGameFactory gameFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            ITaskEx taskEx)
        {
            _gameFactory = gameFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _taskEx = taskEx;
        }

        public IGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup)
        {
            return new GameSetupViewModel(
                _gameFactory,
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator,
                alternateGameSetup,
                _taskEx);
        }
    }
}