using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.Views.WorldMap
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame _game;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private ITurn _currentTurn;

        public GameEngine(IGame game, IWorldMapViewModelFactory worldMapViewModelFactory)
        {
            _game = game;
            _worldMapViewModelFactory = worldMapViewModelFactory;

            //_currentTurn = _game.GetNextTurn();
        }

        public WorldMapViewModel GetWorldMapViewModel()
        {
            var worldMap = _game.GetWorldMap();

            return _worldMapViewModelFactory.Create(worldMap, SelectTerritory);
        }

        private void SelectTerritory(ITerritory territory)
        {
            _currentTurn.Select(territory.Location);
        }
    }
}