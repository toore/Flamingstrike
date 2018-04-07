using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Setup.TerritorySelection
{
    public interface ITerritorySelector
    {
        IReadOnlyList<Territory> GetTerritories();
        Player GetPlayer();
        void PlaceArmyInRegion(IRegion region);
        int GetArmiesLeftToPlace();
    }

    public class TerritorySelector : ITerritorySelector
    {
        private readonly IArmyPlacer _armyPlacer;
        private readonly AlternateGameSetupData _alternateGameSetupData;
        private readonly Setup.Player _currentPlayer;

        public TerritorySelector(IArmyPlacer armyPlacer, AlternateGameSetupData alternateGameSetupData, Setup.Player currentPlayer)
        {
            _armyPlacer = armyPlacer;
            _alternateGameSetupData = alternateGameSetupData;
            _currentPlayer = currentPlayer;
        }

        public IReadOnlyList<Territory> GetTerritories()
        {
            return _alternateGameSetupData.Territories
                .Select(x => new Territory(x.Region, x.Player.Name, x.Armies, x.Player == _currentPlayer))
                .ToList();
        }

        public Player GetPlayer()
        {
            return new Player(_currentPlayer.Name, _currentPlayer.ArmiesToPlace);
        }

        public void PlaceArmyInRegion(IRegion region)
        {
            _armyPlacer.PlaceArmyInRegion(_currentPlayer, region, _alternateGameSetupData);
        }

        public int GetArmiesLeftToPlace()
        {
            return _currentPlayer.ArmiesToPlace;
        }
    }
}