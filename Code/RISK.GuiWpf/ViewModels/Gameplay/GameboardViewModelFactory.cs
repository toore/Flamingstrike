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
        private readonly IGameOverEvaluater _gameOverEvaluater;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITurnPhaseFactory _turnPhaseFactory;

        public GameboardViewModelFactory(
            Locations locations,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelUpdater territoryViewModelUpdater,
            IGameOverEvaluater gameOverEvaluater,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IResourceManagerWrapper resourceManagerWrapper,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            ITurnPhaseFactory turnPhaseFactory)
        {
            _locations = locations;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _gameOverEvaluater = gameOverEvaluater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _resourceManagerWrapper = resourceManagerWrapper;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _turnPhaseFactory = turnPhaseFactory;
        }

        public IGameboardViewModel Create(IGame game)
        {
            return new GameboardViewModel(
                game,
                _locations.GetAll(),
                _worldMapViewModelFactory,
                _territoryViewModelUpdater,
                _gameOverEvaluater,
                _windowManager,
                _gameOverViewModelFactory,
                _resourceManagerWrapper,
                _dialogManager,
                _eventAggregator,
                _turnPhaseFactory);
        }
    }
}