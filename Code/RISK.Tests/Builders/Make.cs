namespace RISK.Tests.Builders
{
    public static class Make
    {
        public static TerritoryBuilder Territory => new TerritoryBuilder();
        public static CardBuilder Card => new CardBuilder();
        public static GameSetupBuilder GameSetup => new GameSetupBuilder();
    }
}