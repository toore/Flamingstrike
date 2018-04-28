using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public interface IDraftArmiesPhase
    {
        string CurrentPlayerName { get; }
        IReadOnlyList<Territory> Territories { get; }
        IReadOnlyList<Player> Players { get; }
        int NumberOfArmiesToDraft { get; }
        IReadOnlyList<Region> RegionsAllowedToDraftArmies { get; }
        void PlaceDraftArmies(Region region, int numberOfArmies);
    }
}