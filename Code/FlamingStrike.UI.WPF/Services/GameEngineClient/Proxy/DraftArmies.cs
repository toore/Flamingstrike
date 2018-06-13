using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class DraftArmies
    {
        public DraftArmies(string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, int numberOfArmiesToDraft, IReadOnlyList<Region> regionsAllowedToDraftArmies)
        {
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            NumberOfArmiesToDraft = numberOfArmiesToDraft;
            RegionsAllowedToDraftArmies = regionsAllowedToDraftArmies;
        }

        public string CurrentPlayerName { get; }
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }
        public int NumberOfArmiesToDraft { get; }
        public IReadOnlyList<Region> RegionsAllowedToDraftArmies { get; }
    }
}