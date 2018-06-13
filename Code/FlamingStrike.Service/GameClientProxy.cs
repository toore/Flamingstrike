using System;
using System.Linq;
using FlamingStrike.GameEngine.Play;
using Microsoft.AspNetCore.SignalR;

namespace FlamingStrike.Service
{
    public class GameClientProxy : IGameObserver
    {
        private readonly GameEngineHub _gameEngineHub;

        public GameClientProxy(GameEngineHub gameEngineHub)
        {
            _gameEngineHub = gameEngineHub;
        }

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            _gameEngineHub.Clients.All.SendAsync(
                "DraftArmies", new
                    {
                        CurrentPlayerName = (string)draftArmiesPhase.CurrentPlayerName,
                        Territories = draftArmiesPhase.Territories.Select(t => t.MapToDto()),
                        Players = draftArmiesPhase.Players.Select(p => p.MapToDto())
                    });
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