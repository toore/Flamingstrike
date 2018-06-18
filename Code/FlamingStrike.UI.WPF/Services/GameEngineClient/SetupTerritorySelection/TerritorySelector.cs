using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection
{
    public interface ITerritorySelector
    {
        IReadOnlyList<Territory> Territories { get; }
        string Player { get; }
        void PlaceArmyInRegion(Region region);
        int ArmiesLeftToPlace { get; }
    }

    public class TerritorySelector : ITerritorySelector
    {
        private readonly IArmyPlacer _armyPlacer;

        public TerritorySelector(IArmyPlacer armyPlacer, string player, int armiesLeftToPlace, IReadOnlyList<Territory> territories)
        {
            Player = player;
            ArmiesLeftToPlace = armiesLeftToPlace;
            _armyPlacer = armyPlacer;
            Territories = territories;
        }

        public string Player { get; }

        public int ArmiesLeftToPlace { get; }

        public IReadOnlyList<Territory> Territories { get; }

        public void PlaceArmyInRegion(Region region)
        {
            _armyPlacer.PlaceArmyInRegion(region);
        }
    }
}