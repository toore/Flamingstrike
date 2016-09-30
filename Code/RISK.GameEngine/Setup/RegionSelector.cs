using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Setup
{
    public interface IRegionSelector
    {
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IRegion> SelectableRegions { get; }
        string PlayerName { get; }
        void SelectRegion(IRegion selectedRegion);
        int GetArmiesLeftToPlace();
    }

    public class RegionSelector : IRegionSelector
    {
        private readonly IAlternateGameSetup _gameSetup;
        private readonly IReadOnlyList<Territory> _territories;
        private readonly IPlayer _player;

        public RegionSelector(IAlternateGameSetup gameSetup, IReadOnlyList<Territory> territories, IReadOnlyList<IRegion> selectableRegions, IPlayer player)
        {
            _gameSetup = gameSetup;
            _territories = territories;
            SelectableRegions = selectableRegions;
            _player = player;
        }

        public IReadOnlyList<ITerritory> Territories => _territories;
        public IReadOnlyList<IRegion> SelectableRegions { get; }
        public string PlayerName => _player.Name;

        public void SelectRegion(IRegion selectedRegion)
        {
            _gameSetup.PlaceArmy(selectedRegion);
        }

        public int GetArmiesLeftToPlace()
        {
            return _player.ArmiesToPlace;
        }
    }
}