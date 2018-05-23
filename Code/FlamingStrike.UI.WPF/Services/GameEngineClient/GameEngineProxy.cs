using System;
using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.Territory;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public class GameEngineProxy : IGameEngineClientProxy
    {
        public async void Setup(IAlternateGameSetupObserver alternateGameSetupObserver, IEnumerable<string> players)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:60643/hubs/gameengine")
                //.WithUrl("https://localhost:44391/hubs/gameengine")
                .ConfigureLogging(cfg => cfg.AddConsole())
                .Build();

            hubConnection.On<SelectRegionRequest>("SelectRegion", dto => OnSelectRegion(alternateGameSetupObserver, hubConnection, dto));
            hubConnection.On<GamePlaySetup>("NewGamePlaySetup", alternateGameSetupObserver.NewGamePlaySetup);

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("RunSetup", players);
        }

        private static void OnSelectRegion(IAlternateGameSetupObserver alternateGameSetupObserver, HubConnection hubConnection, SelectRegionRequest dto)
        {
            var territorySelector = new TerritorySelector(new ArmyPlacerProxy(hubConnection), dto.Player, dto.ArmiesLeftToPlace, dto.Territories);
            alternateGameSetupObserver.SelectRegion(territorySelector);
        }

        public void StartGame(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            throw new NotImplementedException();
        }
    }

    public class ArmyPlacerProxy : IArmyPlacer
    {
        private readonly HubConnection _connection;

        public ArmyPlacerProxy(HubConnection connection)
        {
            _connection = connection;
        }

        public async void PlaceArmyInRegion(Region selectedRegion)
        {
            await _connection.SendAsync("PlaceArmyInRegion", Enum.GetName(typeof(Region), selectedRegion));
        }
    }

    public class SelectRegionRequest
    {
        public string Player { get; set; }
        public int ArmiesLeftToPlace { get; set; }
        public IReadOnlyList<Territory> Territories { get; set; }
    }
}