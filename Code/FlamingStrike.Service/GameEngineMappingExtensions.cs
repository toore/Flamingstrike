using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using FlamingStrike.Service.Setup;

namespace FlamingStrike.Service
{
    public static class GameEngineMappingExtensions
    {
        public static PlayerName MapToEngine(this string playerName)
        {
            return new PlayerName(playerName);
        }

        public static TerritoryDto MapToDto(this Territory territory)
        {
            return new TerritoryDto(territory.Region.Name, (string)territory.Name, territory.Armies, territory.IsSelectable);
        }

        public static Setup.Finished.TerritoryDto MapToDto(this GameEngine.Setup.Finished.Territory territory)
        {
            return new Setup.Finished.TerritoryDto(territory.Region.Name, (string)territory.Name, territory.Armies);
        }
    }
}