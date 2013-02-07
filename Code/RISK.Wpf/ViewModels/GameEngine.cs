using GuiWpf.Views.WorldMap;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public class GameEngine : IGameEngine
    {
        private IGame _game;
        private IWorldMapViewModelFactory _worldMapViewModelFactory;
        private ITurn _currentTurn;
        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public GameEngine(IGame game, IWorldMapViewModelFactory worldMapViewModelFactory)
        {
            _game = game;
            _worldMapViewModelFactory = worldMapViewModelFactory;

            GetNextTurn();

            CreateWorldMapViewModel();
        }

        private void GetNextTurn()
        {
            _currentTurn = _game.GetNextTurn();
        }

        private void CreateWorldMapViewModel()
        {
            var worldMap = _game.GetWorldMap();

            WorldMapViewModel = _worldMapViewModelFactory.Create(worldMap, SelectTerritory);
        }

        private void SelectTerritory(ITerritory territory)
        {
            _currentTurn.Select(territory.Location);

            //_gameEngineSubscriber.UpdateWorldMapViewModel(GetWorldMapViewModel());
        }
    }
}