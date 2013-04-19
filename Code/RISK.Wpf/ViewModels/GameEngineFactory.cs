using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.WorldMap;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class GameEngineFactory : IGameEngineFactory
    {
        private readonly IGameFactory _gameFactory;
        private readonly ILocationProvider _locationProvider;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;

        public GameEngineFactory(IGameFactory gameFactory, ILocationProvider locationProvider, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater)
        {
            _gameFactory = gameFactory;
            _locationProvider = locationProvider;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
        }

        public IGameEngine Create()
        {
            var game = _gameFactory.Create();

            return new GameEngine(game, _locationProvider, _worldMapViewModelFactory, _territoryViewModelUpdater);
        }
    }
}