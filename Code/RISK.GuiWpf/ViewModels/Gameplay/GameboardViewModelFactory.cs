using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly IWorldMap _worldMap;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;

        public GameboardViewModelFactory(
            IWorldMap worldMap,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMap = worldMap;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IGameboardViewModel Create(IGameAdapter gameAdapter)
        {
            return new GameboardViewModel(
                _worldMap,
                gameAdapter,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }
    }
}