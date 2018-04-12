namespace FlamingStrike.GameEngine.Setup
{
    public interface IArmyPlacer
    {
        void PlaceArmyInRegion(Player currentPlayer, Region selectedRegion, AlternateGameSetupData alternateGameSetupData);
    }
}