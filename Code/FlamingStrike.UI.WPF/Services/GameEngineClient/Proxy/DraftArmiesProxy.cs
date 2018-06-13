using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using Microsoft.AspNetCore.SignalR.Client;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class DraftArmiesProxy : IDraftArmiesPhase
    {
        private readonly HubConnection _hubConnection;

        public DraftArmiesProxy(HubConnection hubConnection, string currentPlayerName, IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, int numberOfArmiesToDraft, IReadOnlyList<Region> regionsAllowedToDraftArmies)
        {
            _hubConnection = hubConnection;
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

        public void PlaceDraftArmies(Region region, int numberOfArmies)
        {
            _hubConnection.SendAsync(
                "PlaceDraftArmies", new
                    {
                        Region = region.MapToDto(),
                        NumberOfArmies = numberOfArmies
                    });
        }
    }
}