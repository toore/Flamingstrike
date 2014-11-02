using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly Locations _locations;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;

        public GameboardViewModelFactory(
            Locations locations,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelUpdater territoryViewModelUpdater,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _locations = locations;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IGameboardViewModel Create(IGame game)
        {
            return new GameboardViewModel(
                game,
                _locations.GetAll(),
                _worldMapViewModelFactory,
                _territoryViewModelUpdater,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }
    }
}