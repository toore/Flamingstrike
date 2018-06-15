using System;
using System.Collections.Generic;
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
        private IAttackPhase _attackPhase;

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
            _attackPhase = attackPhase;
            _gameEngineHub.Clients.All.SendAsync(
                "Attack", new
                    {
                        CurrentPlayerName = (string)attackPhase.CurrentPlayerName,
                        Territories = attackPhase.Territories.Select(t => t.MapToDto()),
                        Players = attackPhase.Players.Select(p => p.MapToDto()),
                        RegionsThatCanBeSourceForAttackOrFortification = attackPhase.RegionsThatCanBeSourceForAttackOrFortification.Select(x => x.Name),
                    });
        }

        public void Attack(Region attackingRegion, Region defendingRegion)
        {
            _attackPhase.Attack(attackingRegion, defendingRegion);
        }

        public void Fortify(Region sourceRegion, Region destinationRegion, int armies)
        {
            _attackPhase.Fortify(sourceRegion, destinationRegion, armies);
        }

        public IEnumerable<Region> GetRegionsThatCanBeAttacked(Region sourceRegion)
        {
            return _attackPhase.GetRegionsThatCanBeAttacked(sourceRegion);
        }

        public IEnumerable<Region> GetRegionsThatCanBeFortified(Region sourceRegion)
        {
            return _attackPhase.GetRegionsThatCanBeFortified(sourceRegion);
        }

        public void EndTurn()
        {
            _attackPhase.EndTurn();
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