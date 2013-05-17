using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
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
        private readonly IUserNotifier _userNotifier;

        public GameboardViewModelFactory(ILocationProvider locationProvider, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater, IGameOverEvaluater gameOverEvaluater, IWindowManager windowManager, IGameOverViewModelFactory gameOverViewModelFactory, IUserNotifier userNotifier)
        {
            _locationProvider = locationProvider;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _gameOverEvaluater = gameOverEvaluater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _userNotifier = userNotifier;
        }

        public IGameboardViewModel Create(IGame game)
        {
            return new GameboardViewModel(game, _locationProvider, _worldMapViewModelFactory, _territoryViewModelUpdater, _gameOverEvaluater, _windowManager, _gameOverViewModelFactory, _userNotifier);
        }
    }
}