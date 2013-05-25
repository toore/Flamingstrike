using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly ILocationProvider _locationProvider;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly IGameOverEvaluater _gameOverEvaluater;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;
        private readonly IDialogManager _dialogManager;
        private readonly IGameEventAggregator _gameEventAggregator;

        public GameboardViewModelFactory(
            ILocationProvider locationProvider,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelUpdater territoryViewModelUpdater,
            IGameOverEvaluater gameOverEvaluater,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IResourceManagerWrapper resourceManagerWrapper,
            IDialogManager dialogManager,
            IGameEventAggregator gameEventAggregator)
        {
            _locationProvider = locationProvider;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _gameOverEvaluater = gameOverEvaluater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _resourceManagerWrapper = resourceManagerWrapper;
            _dialogManager = dialogManager;
            _gameEventAggregator = gameEventAggregator;
        }

        public IGameboardViewModel Create(IGame game)
        {
            return new GameboardViewModel(
                game,
                _locationProvider,
                _worldMapViewModelFactory,
                _territoryViewModelUpdater,
                _gameOverEvaluater,
                _windowManager,
                _gameOverViewModelFactory,
                _resourceManagerWrapper,
                _dialogManager,
                _gameEventAggregator);
        }
    }
}