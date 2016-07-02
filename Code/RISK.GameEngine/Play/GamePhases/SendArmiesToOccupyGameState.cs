using System;
using System.Collections.Generic;
using RISK.Core;
using RISK.GameEngine.Extensions;

namespace RISK.GameEngine.Play.GamePhases
{
    public class SendArmiesToOccupyGameState : IGameState
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly GameData _gameData;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;

        public SendArmiesToOccupyGameState(
            IGameStateConductor gameStateConductor,
            IGameDataFactory gameDataFactory,
            ITerritoryOccupier territoryOccupier,
            GameData gameData,
            IRegion attackingRegion,
            IRegion occupiedRegion)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _territoryOccupier = territoryOccupier;
            _gameData = gameData;
            _attackingRegion = attackingRegion;
            _occupiedRegion = occupiedRegion;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public bool CanSendAdditionalArmiesToOccupy()
        {
            return true;
        }

        public int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy()
        {
            return _gameData.Territories.GetTerritory(_attackingRegion).GetNumberOfArmiesThatCanBeSentToOccupy();
        }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            var updatedTerritories = _territoryOccupier.SendInAdditionalArmiesToOccupy(_gameData.Territories, _attackingRegion, _occupiedRegion, numberOfArmies);
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, updatedTerritories, Deck);

            _gameStateConductor.ContinueWithAttackPhase(gameData, TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return false;
        }

        public int GetNumberOfArmiesToDraft()
        {
            throw new InvalidOperationException();
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            throw new InvalidOperationException();
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return false;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            throw new InvalidOperationException();
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return false;
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            throw new InvalidOperationException();
        }

        public bool CanEndTurn()
        {
            return false;
        }

        public void EndTurn()
        {
            throw new InvalidOperationException();
        }
    }
}