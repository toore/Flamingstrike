using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.WorldMap;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class GameEngineFactory : IGameEngineFactory
    {
        private readonly IGame _game;
        private readonly ILocationRepository _locationRepository;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;

        public GameEngineFactory(IGame game, ILocationRepository locationRepository, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater)
        {
            _game = game;
            _locationRepository = locationRepository;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _territoryViewModelUpdater = territoryViewModelUpdater;
        }

        public IGameEngine Create()
        {
            return new GameEngine(_game, _locationRepository, _worldMapViewModelFactory, _territoryViewModelUpdater);
        }
    }
}