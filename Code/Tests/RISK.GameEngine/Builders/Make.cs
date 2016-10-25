namespace Tests.RISK.GameEngine.Builders
{
    public static class Make
    {
        public static RegionBuilder Region => new RegionBuilder();
        public static ContinentBuilder Continent => new ContinentBuilder();
        public static GamePlaySetupBuilder GamePlaySetup => new GamePlaySetupBuilder();
        public static TerritoryBuilder Territory => new TerritoryBuilder();
        public static DicesBuilder Dices => new DicesBuilder();
        public static ArmiesLostBuilder ArmiesLost => new ArmiesLostBuilder();
        public static PlayerBuilder Player => new PlayerBuilder();
        public static PlayerGameDataBuilder PlayerGameData => new PlayerGameDataBuilder();
        public static GameDataBuilder GameData => new GameDataBuilder();
        public static DeckBuilder Deck => new DeckBuilder();
    }
}