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
        private readonly IReadOnlyList<Territory> _territories;
        private readonly PlayerSetupData _playerSetupData;

        public PlaceArmyRegionSelector(IArmyPlacer armyPlacer, IReadOnlyList<Territory> territories, IReadOnlyList<IRegion> selectableRegions, PlayerSetupData playerSetupData)
        {
            _armyPlacer = armyPlacer;
            _territories = territories;
            SelectableRegions = selectableRegions;
            _playerSetupData = playerSetupData;
        }

        public IReadOnlyList<ITerritory> Territories => _territories;
        public IReadOnlyList<IRegion> SelectableRegions { get; }
        public IPlayer Player => _playerSetupData.Player;

        public void PlaceArmyInRegion(IRegion region)
        {
            _armyPlacer.PlaceArmyInRegion(region);
        }

        public int GetArmiesLeftToPlace()
        {
            return _playerSetupData.ArmiesToPlace;
        }
    }
}