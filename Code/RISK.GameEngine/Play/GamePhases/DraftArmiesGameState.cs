using System;
using System.Collections.Generic;
using RISK.Core;
using RISK.GameEngine.Extensions;

namespace RISK.GameEngine.Play.GamePhases
{
    public class DraftArmiesGameState : IGameState
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly int _numberOfArmiesToDraft;
        private readonly IArmyDrafter _armyDrafter;
        private readonly GameData _gameData;

        public DraftArmiesGameState(
            IGameStateConductor gameStateConductor,
            IGameDataFactory gameDataFactory,
            IArmyDrafter armyDrafter,
            GameData gameData,
            int numberOfArmiesToDraft)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
            _armyDrafter = armyDrafter;
            _gameData = gameData;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return IsCurrentPlayerOccupyingRegion(region);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _gameData.CurrentPlayer == _gameData.Territories.GetTerritory(region).Player;
        }

        public int GetNumberOfArmiesToDraft()
        {
            return _numberOfArmiesToDraft;
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            if (numberOfArmiesToPlace > _numberOfArmiesToDraft)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArmiesToPlace));
            }
            if (!IsCurrentPlayerOccupyingRegion(region))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(region)}.");
            }

            var updatedTerritories = _armyDrafter.PlaceDraftArmies(_gameData.Territories, region, numberOfArmiesToPlace);

            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, _gameData.Players, updatedTerritories, Deck);
            var numberOfArmiesLeftToPlace = _numberOfArmiesToDraft - numberOfArmiesToPlace;

            if (numberOfArmiesLeftToPlace > 0)
            {
                _gameStateConductor.ContinueToDraftArmies(gameData, numberOfArmiesLeftToPlace);
            }
            else
            {
                _gameStateConductor.ContinueWithAttackPhase(gameData, TurnConqueringAchievement.NoTerritoryHasBeenConquered);
            }
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return false;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            throw new InvalidOperationException();
        }

        public bool CanSendAdditionalArmiesToOccupy()
        {
            return false;
        }

        public int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy()
        {
            throw new InvalidOperationException();
        }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            throw new InvalidOperationException();
        }

        public bool CanFreeMove()
        {
            return false;
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