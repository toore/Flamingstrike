using System;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using Microsoft.AspNetCore.SignalR;

namespace FlamingStrike.Service
{
    public class GameClientProxy : IGameObserver
    {
        private readonly GameEngineHub _gameEngineHub;
        private IDraftArmiesPhase _draftArmiesPhase;

        public GameClientProxy(GameEngineHub gameEngineHub)
        {
            _gameEngineHub = gameEngineHub;
        }

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            _draftArmiesPhase = draftArmiesPhase;
            _gameEngineHub.Clients.All.SendAsync(
                "DraftArmies", new
                    {
                        CurrentPlayerName = (string)draftArmiesPhase.CurrentPlayerName,
                        Territories = draftArmiesPhase.Territories.Select(t => t.MapToDto()),
                        Players = draftArmiesPhase.Players.Select(p => p.MapToDto()),
                        draftArmiesPhase.NumberOfArmiesToDraft,
                        RegionsAllowedToDraftArmies = draftArmiesPhase.RegionsAllowedToDraftArmies.Select(x => x.Name)
                    });
        }

        public void PlaceDraftArmies(Region region, int numberOfArmies)
        {
            _draftArmiesPhase.PlaceDraftArmies(region, numberOfArmies);
        }

        public void Attack(IAttackPhase attackPhase)
        {
            throw new NotImplementedException();
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            throw new NotImplementedException();
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            throw new NotImplementedException();
        }

        public void GameOver(IGameOverState gameOverState)
        {
            throw new NotImplementedException();
        }
    }
}