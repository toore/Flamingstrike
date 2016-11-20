using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Setup
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
        private readonly AlternateGameSetupData _alternateGameSetupData;
        private readonly PlayerSetupData _playerSetupData;

        public PlaceArmyRegionSelector(IArmyPlacer armyPlacer, AlternateGameSetupData alternateGameSetupData, IReadOnlyList<IRegion> selectableRegions, PlayerSetupData playerSetupData)
        {
            _armyPlacer = armyPlacer;
            _alternateGameSetupData = alternateGameSetupData;
            SelectableRegions = selectableRegions;
            _playerSetupData = playerSetupData;
        }

        public IReadOnlyList<ITerritory> Territories => _alternateGameSetupData.Territories;
        public IReadOnlyList<IRegion> SelectableRegions { get; }
        public IPlayer Player => _playerSetupData.Player;

        public void PlaceArmyInRegion(IRegion region)
        {
            _armyPlacer.PlaceArmyInRegion(_playerSetupData.Player, region, _alternateGameSetupData);
        }

        public int GetArmiesLeftToPlace()
        {
            return _playerSetupData.ArmiesToPlace;
        }
    }
}