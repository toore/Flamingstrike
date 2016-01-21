using RISK.Application.Play;

namespace RISK.Tests.Builders
{
    public static class Make
    {
        public static TerritoryGeographyBuilder TerritoryGeography => new TerritoryGeographyBuilder();
        public static CardBuilder Card => new CardBuilder();
        public static GamePlaySetupBuilder GamePlaySetup => new GamePlaySetupBuilder();
        public static TerritoryBuilder Territory => new TerritoryBuilder();
        public static PlayerBuilder Player => new PlayerBuilder();
    }
}