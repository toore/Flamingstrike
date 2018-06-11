using System;
using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.Territory;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public class GameEngineProxy : GameEngineClientProxyBase
    {
        private HubConnection _hubConnection;

        private async void LazyConnect()
        {
            if (_hubConnection != null)
            {
                return;

            }
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:60643/hubs/gameengine")
                //.WithUrl("https://localhost:44391/hubs/gameengine")
                .ConfigureLogging(cfg => cfg.AddConsole())
                .Build();

            _hubConnection.On<SelectRegionRequest>("SelectRegion", SelectRegionRequest);
            _hubConnection.On<GamePlaySetup>("NewGamePlaySetup", GamePlaySetup);

            await _hubConnection.StartAsync();
        }

        public override async void Setup(IEnumerable<string> players)
        {
            LazyConnect();

            await _hubConnection.SendAsync("RunSetup", players);
        }

        public override void StartGame(IGamePlaySetup gamePlaySetup)
        {
            LazyConnect();

            throw new NotImplementedException();
        }

        private void SelectRegionRequest(SelectRegionRequest dto)
        {
            var territorySelector = new TerritorySelector(new ArmyPlacerProxy(_hubConnection), dto.Player, dto.ArmiesLeftToPlace, dto.Territories);
            _territorySelectorSubject.OnNext(territorySelector);
        }

        private void GamePlaySetup(GamePlaySetup gamePlaySetup)
        {
            _gamePlaySetupSubject.OnNext(gamePlaySetup);
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