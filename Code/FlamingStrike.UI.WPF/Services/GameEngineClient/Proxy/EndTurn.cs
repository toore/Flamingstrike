using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class EndTurn
    {
        public EndTurn(string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players)
        {
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
        }

        public string CurrentPlayerName { get; }
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }
    }
}