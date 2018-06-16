using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class SendArmiesToOccupyProxy : ISendArmiesToOccupyPhase
    {
        private readonly HubConnection _hubConnection;

        public SendArmiesToOccupyProxy(HubConnection hubConnection, string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, Region attackingRegion, Region occupiedRegion)
        {
            _hubConnection = hubConnection;
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

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            _hubConnection.SendAsync("SendAdditionalArmiesToOccupy", numberOfArmies);
        }
    }
}