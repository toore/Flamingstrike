using GuiWpf.Views.WorldMapView;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.Views.Main
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame _game;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;

        public GameEngine(IGame game, IWorldMapViewModelFactory worldMapViewModelFactory)
        {
            _game = game;
            _worldMapViewModelFactory = worldMapViewModelFactory;
        }

        public WorldMapViewModel GetWorldMapViewModel()
        {
            var worldMap = _game.GetWorldMap();

            return _worldMapViewModelFactory.Create(worldMap, SelectTerritory);
        }

        private void SelectTerritory(ITerritory territory)
        {
            throw new System.NotImplementedException();
        }
    }
}