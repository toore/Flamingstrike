using RISK.Application.Play;

namespace RISK.Tests.Builders
{
    public static class Make
    {
        public static TerritoryBuilder Territory => new TerritoryBuilder();
        public static CardBuilder Card => new CardBuilder();
        public static GameSetupBuilder GameSetup => new GameSetupBuilder();
        public static GameboardTerritoryBuilder GameboardTerritory => new GameboardTerritoryBuilder();
    }
}