using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class SendArmiesToOccupy
    {
        public SendArmiesToOccupy(string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, Region attackingRegion, Region occupiedRegion)
        {
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            AttackingRegion = attackingRegion;
            OccupiedRegion = occupiedRegion;
        }

        public string CurrentPlayerName { get; }
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }
        public Region AttackingRegion { get; }
        public Region OccupiedRegion { get; }
    }
}