using System.Collections.Generic;

namespace RISK.GameEngine.Setup
{
    public interface IPlaceArmyRegionSelector
    {
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IRegion> SelectableRegions { get; }
        string PlayerName { get; }
        void PlaceArmyInRegion(IRegion region);
        int GetArmiesLeftToPlace();
    }

    public class PlaceArmyRegionSelector : IPlaceArmyRegionSelector
    {
        private readonly IArmyPlacer _armyPlacer;
        private readonly IReadOnlyList<Territory> _territories;
        private readonly IPlayer _player;

        public PlaceArmyRegionSelector(IArmyPlacer armyPlacer, IReadOnlyList<Territory> territories, IReadOnlyList<IRegion> selectableRegions, IPlayer player)
        {
            _armyPlacer = armyPlacer;
            _territories = territories;
            SelectableRegions = selectableRegions;
            _player = player;
        }

        public IReadOnlyList<ITerritory> Territories => _territories;
        public IReadOnlyList<IRegion> SelectableRegions { get; }
        public string PlayerName => _player.Name;

        public void PlaceArmyInRegion(IRegion region)
        {
            _armyPlacer.PlaceArmyInRegion(region);
        }

        public int GetArmiesLeftToPlace()
        {
            return _player.ArmiesToPlace;
        }
    }
}