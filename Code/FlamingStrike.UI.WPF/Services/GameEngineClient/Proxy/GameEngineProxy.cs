using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class GameEngineProxy : GameEngineClientBase
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

            _hubConnection.On<SelectRegion>("SelectRegion", SelectRegion);
            _hubConnection.On<GamePlaySetup>("NewGamePlaySetup", NewGamePlaySetup);

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
            await _hubConnection.SendAsync(
                "StartGame", new
                    {
                        Players = gamePlaySetup.GetPlayers(),
                        Territories = gamePlaySetup.GetTerritories().Select(
                            x => new
                                {
                                    Region = x.Region.MapToDto(),
                                    PlayerName = x.Player,
                                    x.Armies
                                })
                    });
        }

        private void SelectRegion(SelectRegion dto)
        {
            var territorySelector = new TerritorySelector(new ArmyPlacerProxy(_hubConnection), dto.Player, dto.ArmiesLeftToPlace, dto.Territories);
            _territorySelectorSubject.OnNext(territorySelector);
        }

        private void NewGamePlaySetup(GamePlaySetup gamePlaySetup)
        {
            _gamePlaySetupSubject.OnNext(gamePlaySetup);
        }

        private void DraftArmies(DraftArmies dto)
        {
            var draftArmiesPhase = new DraftArmiesProxy(_hubConnection, dto.CurrentPlayerName, dto.Territories, dto.Players, dto.NumberOfArmiesToDraft, dto.RegionsAllowedToDraftArmies);
            _draftArmiesPhaseSubject.OnNext(draftArmiesPhase);
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

    public class Attack {}

    internal class SendArmiesToOccupy {}

    internal class EndTurn {}

    internal class GameOver {}
}