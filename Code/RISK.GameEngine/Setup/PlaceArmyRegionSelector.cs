using System.Collections.Generic;

namespace RISK.GameEngine.Setup
{
    public interface IPlaceArmyRegionSelector
    {
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IRegion> SelectableRegions { get; }
        IPlayer Player { get; }
        void PlaceArmyInRegion(IRegion region);
        int GetArmiesLeftToPlace();
    }

    public class PlaceArmyRegionSelector : IPlaceArmyRegionSelector
    {
        private readonly IArmyPlacer _armyPlacer;
        private readonly TerritoriesAndPlayers _territoriesAndPlayers;
        private readonly PlayerSetupData _playerSetupData;

        public PlaceArmyRegionSelector(IArmyPlacer armyPlacer, TerritoriesAndPlayers territoriesAndPlayers, IReadOnlyList<IRegion> selectableRegions, PlayerSetupData playerSetupData)
        {
            _armyPlacer = armyPlacer;
            _territoriesAndPlayers = territoriesAndPlayers;
            SelectableRegions = selectableRegions;
            _playerSetupData = playerSetupData;
        }

        public IReadOnlyList<ITerritory> Territories => _territoriesAndPlayers.Territories;
        public IReadOnlyList<IRegion> SelectableRegions { get; }
        public IPlayer Player => _playerSetupData.Player;

        public void PlaceArmyInRegion(IRegion region)
        {
            _armyPlacer.PlaceArmyInRegion(_playerSetupData.Player, region, _territoriesAndPlayers);
        }

        public int GetArmiesLeftToPlace()
        {
            return _playerSetupData.ArmiesToPlace;
        }
    }
}