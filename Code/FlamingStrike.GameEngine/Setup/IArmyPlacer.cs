namespace FlamingStrike.GameEngine.Setup
{
    public interface IArmyPlacer
    {
        void PlaceArmyInRegion(Player currentPlayer, IRegion selectedRegion, AlternateGameSetupData alternateGameSetupData);
    }
}