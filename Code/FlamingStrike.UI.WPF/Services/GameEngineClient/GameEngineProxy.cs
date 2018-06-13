using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

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

            _hubConnection.On<DraftArmies>("DraftArmies", DraftArmies);
            _hubConnection.On<Attack>("Attack", Attack);
            _hubConnection.On<SendArmiesToOccupy>("SendArmiesToOccupy", SendArmiesToOccupy);
            _hubConnection.On<EndTurn>("EndTurn", EndTurn);
            _hubConnection.On<GameOver>("GameOver", GameOver);

            await _hubConnection.StartAsync();
        }

        public override async void Setup(IEnumerable<string> players)
        {
            LazyConnect();

            await _hubConnection.SendAsync("RunSetup", players);
        }

        public override async void StartGame(IGamePlaySetup gamePlaySetup)
        {
            LazyConnect();

            await _hubConnection.SendAsync(
                "StartGame", new
                    {
                        Players = gamePlaySetup.GetPlayers(),
                        Territories = gamePlaySetup.GetTerritories().Select(
                            x => new
                                {
                                    Region = x.Region.MapToDto(),
                                    x.Player,
                                    x.Armies
                                })
                    });
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

        private void DraftArmies(DraftArmies draftArmies)
        {
            throw new NotImplementedException();
            //_draftArmiesPhaseSubject.OnNext(new );
        }

        private void Attack(Attack attack)
        {
            throw new NotImplementedException();
        }

        private void SendArmiesToOccupy(SendArmiesToOccupy sendArmiesToOccupy)
        {
            throw new NotImplementedException();
        }

        private void EndTurn(EndTurn endTurn)
        {
            throw new NotImplementedException();
        }

        private void GameOver(GameOver gameOver)
        {
            throw new NotImplementedException();
        }
    }

    public class DraftArmies {}

    public class Attack {}

    internal class SendArmiesToOccupy {}

    internal class EndTurn {}

    internal class GameOver {}

    public static class GameEngineProxyExtensions
    {
        public static string MapToDto(this Region region)
        {
            return Enum.GetName(typeof(Region), region);
        }
    }
}