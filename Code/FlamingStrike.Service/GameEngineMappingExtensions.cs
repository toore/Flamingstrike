using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.Service.Play;
using Territory = FlamingStrike.GameEngine.Setup.TerritorySelection.Territory;
using TerritoryDto = FlamingStrike.Service.Setup.TerritoryDto;

namespace FlamingStrike.Service
{
    public static class GameEngineMappingExtensions
    {
        public static PlayerName MapToEngine(this string playerName)
        {
            return new PlayerName(playerName);
        }

        public static Region MapRegionNameToEngine(this string regionName)
        {
            return (Region)typeof(Region).GetFields()
                .Where(x => x.IsPublic && x.IsStatic)
                .Single(x => x.Name == regionName)
                .GetValue(typeof(Region));
        }

        public static TerritoryDto MapToDto(this Territory territory)
        {
            return new TerritoryDto(territory.Region.Name, (string)territory.Name, territory.Armies, territory.IsSelectable);
        }

        public static Setup.Finished.TerritoryDto MapToDto(this GameEngine.Setup.Finished.Territory territory)
        {
            return new Setup.Finished.TerritoryDto(territory.Region.Name, (string)territory.Name, territory.Armies);
        }

        public static GameEngine.Setup.Finished.Territory MapToEngine(this Play.TerritoryDto territory)
        {
            return new GameEngine.Setup.Finished.Territory(territory.Region.MapRegionNameToEngine(), territory.PlayerName.MapToEngine(), territory.Armies);
        }

        public static Play.TerritoryDto MapToDto(this ITerritory territory)
        {
            return new Play.TerritoryDto(territory.Region.Name, (string)territory.PlayerName, territory.Armies);
        }

        public static PlayerDto MapToDto(this IPlayer player)
        {
            return new PlayerDto((string)player.Name, player.Cards.Count);
        }
    }
}