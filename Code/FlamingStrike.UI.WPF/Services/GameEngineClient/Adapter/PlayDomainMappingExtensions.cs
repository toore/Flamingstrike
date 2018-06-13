using FlamingStrike.GameEngine.Play;
using Player = FlamingStrike.UI.WPF.Services.GameEngineClient.Play.Player;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.Play.Territory;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Adapter
{
    public static class PlayDomainMappingExtensions
    {
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