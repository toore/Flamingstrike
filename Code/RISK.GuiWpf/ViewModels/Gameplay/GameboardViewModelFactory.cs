using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly ITerritoryViewModelColorInitializer _territoryViewModelColorInitializer;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;

        public GameboardViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelColorInitializer territoryViewModelColorInitializer,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelColorInitializer = territoryViewModelColorInitializer;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IGameboardViewModel Create(IGame game)
        {
            return new GameboardViewModel(
                game,
                _worldMapViewModelFactory,
                _territoryViewModelColorInitializer,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }
    }
}