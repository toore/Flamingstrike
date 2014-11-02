namespace RISK.Tests
{
    public static class Make
    {
        public static LocationBuilder Location { get { return new LocationBuilder(); } }
        public static TerritoryBuilder Territory { get{ return new TerritoryBuilder();}}
        public static CardBuilder Card { get { return new CardBuilder();} }
    }
}