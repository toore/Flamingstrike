using System;
using System.Linq;
using System.Reflection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
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
    }
}