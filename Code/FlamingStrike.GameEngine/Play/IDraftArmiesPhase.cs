using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IDraftArmiesPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayer> Players { get; }
        int NumberOfArmiesToDraft { get; }
        IReadOnlyList<Region> RegionsAllowedToDraftArmies { get; }
        void PlaceDraftArmies(Region region, int numberOfArmies);
    }
}