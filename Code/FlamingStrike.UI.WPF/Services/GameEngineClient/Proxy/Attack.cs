using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class Attack
    {
        public Attack(string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, IReadOnlyList<Region> regionsThatCanBeSourceForAttackOrFortification)
        {
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            RegionsThatCanBeSourceForAttackOrFortification = regionsThatCanBeSourceForAttackOrFortification;
        }

        public string CurrentPlayerName { get; }
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }
        public IReadOnlyList<Region> RegionsThatCanBeSourceForAttackOrFortification { get; }
    }
}