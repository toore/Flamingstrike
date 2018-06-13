using System;
using System.Linq;
using System.Reflection;
using FlamingStrike.GameEngine.Play;
using Player = FlamingStrike.UI.WPF.Services.GameEngineClient.Play.Player;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.Play.Territory;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Adapter
{
    public static class GameEngineMappingExtensions
    {
        public static Region MapFromEngine(this GameEngine.Region region)
        {
            return (Region)Enum.Parse(typeof(Region), region.Name, true);
        }

        public static GameEngine.Region MapToEngine(this Region region)
        {
            return typeof(GameEngine.Region).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(x => x.GetValue(null))
                .Cast<GameEngine.Region>()
                .Single(x => x.Name.Equals(Enum.GetName(typeof(Region), region), StringComparison.InvariantCultureIgnoreCase));
        }

        public static Territory MapFromEngine(this ITerritory territory)
        {
            return new Territory(territory.Region.MapFromEngine(), (string)territory.PlayerName, territory.Armies);
        }

        public static Player MapFromEngine(this IPlayer x)
        {
            return new Player((string)x.Name, x.Cards.Count);
        }
    }
}