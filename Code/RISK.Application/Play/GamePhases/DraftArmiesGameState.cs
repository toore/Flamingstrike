using System;
using System.Collections.Generic;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : IGameState
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly int _numberOfArmiesToDraft;
        private readonly IArmyModifier _armyModifier;
        private readonly GameData _gameData;

        public DraftArmiesGameState(
            IGameStateConductor gameStateConductor,
            IGameDataFactory gameDataFactory,
            IArmyModifier armyModifier,
            GameData gameData,
            int numberOfArmiesToDraft)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
            _armyModifier = armyModifier;
            _gameData = gameData;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return Territories.GetTerritory(region).Player == CurrentPlayer;
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

            var updatedTerritories = _armyModifier.PlaceDraftArmies(Territories, region, numberOfArmiesToPlace);

            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, updatedTerritories, Deck);
            var numberOfArmiesLeftToPlace = _numberOfArmiesToDraft - numberOfArmiesToPlace;

            if (numberOfArmiesLeftToPlace > 0)
            {
                _gameStateConductor.ContinueToDraftArmies(gameData, numberOfArmiesLeftToPlace);
            }
            else
            {
                _gameStateConductor.ContinueWithAttackPhase(gameData, ConqueringAchievement.DoNotAwardCardAtEndOfTurn);
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