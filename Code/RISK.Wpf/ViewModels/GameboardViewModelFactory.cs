using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly IGameFactory _gameFactory;
        private readonly ILocationProvider _locationProvider;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;

        public GameboardViewModelFactory(IGameFactory gameFactory, ILocationProvider locationProvider, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater)
        {
            _gameFactory = gameFactory;
            _locationProvider = locationProvider;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
        }

        public IGameboardViewModel Create()
        {
            var game = _gameFactory.Create();

            return new GameboardViewModel(game, _locationProvider, _worldMapViewModelFactory, _territoryViewModelUpdater);
        }
    }
}