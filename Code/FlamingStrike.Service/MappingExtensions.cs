using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using Player = FlamingStrike.Service.Play.Player;
using Territory = FlamingStrike.Service.Setup.Territory;

namespace FlamingStrike.Service
{
    public static class MappingExtensions
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

        public static Territory MapToDto(this GameEngine.Setup.TerritorySelection.Territory territory)
        {
            return new Territory(territory.Region.Name, (string)territory.Name, territory.Armies, territory.IsSelectable);
        }

        public static Setup.Finished.Territory MapToDto(this GameEngine.Setup.Finished.Territory territory)
        {
            return new Setup.Finished.Territory(territory.Region.Name, (string)territory.Name, territory.Armies);
        }

        public static GameEngine.Setup.Finished.Territory MapToEngine(this Play.Territory territory)
        {
            return new GameEngine.Setup.Finished.Territory(territory.Region.MapRegionNameToEngine(), territory.PlayerName.MapToEngine(), territory.Armies);
        }

        public static Play.Territory MapToDto(this ITerritory territory)
        {
            return new Play.Territory(territory.Region.Name, (string)territory.PlayerName, territory.Armies);
        }

        public static Player MapToDto(this IPlayer player)
        {
            return new Player((string)player.Name, player.Cards.Count);
        }
    }
}