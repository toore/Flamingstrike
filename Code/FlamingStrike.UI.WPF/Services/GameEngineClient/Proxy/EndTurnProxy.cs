using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class EndTurnProxy : IEndTurnPhase
    {
        private readonly HubConnection _hubConnection;

        public EndTurnProxy(HubConnection hubConnection, string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players)
        {
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            _hubConnection = hubConnection;
        }

        public string CurrentPlayerName { get; }
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }

        public void EndTurn()
        {
            _hubConnection.SendAsync("EndTurn");
        }
    }
}