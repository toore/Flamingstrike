namespace RISK.Tests.Builders
{
    public static class Make
    {
        public static RegionBuilder Region => new RegionBuilder();
        public static CardBuilder Card => new CardBuilder();
        public static GamePlaySetupBuilder GamePlaySetup => new GamePlaySetupBuilder();
        public static TerritoryBuilder Territory => new TerritoryBuilder();
        public static DicesBuilder Dices => new DicesBuilder();
        public static BattleOutcomeBuilder BattleOutcome => new BattleOutcomeBuilder();
    }
}