using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Core;
using RISK.GameEngine.Extensions;

namespace RISK.GameEngine.Play.GamePhases
{
    public class AttackGameState : IGameState
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly GameData _gameData;
        private ConqueringAchievement _conqueringAchievement = ConqueringAchievement.DoNotAwardCardAtEndOfTurn;

        public AttackGameState(IGameStateConductor gameStateConductor, IGameDataFactory gameDataFactory, IAttacker attacker, IFortifier fortifier, GameData gameData)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _attacker = attacker;
            _fortifier = fortifier;
            _gameData = gameData;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(attackingRegion))
            {
                return false;
            }

            return _attacker.CanAttack(_gameData.Territories, attackingRegion, defendingRegion);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _gameData.CurrentPlayer == _gameData.Territories.GetTerritory(region).Player;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(attackingRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(attackingRegion)}.");
            }

            var attackOutcome = _attacker.Attack(_gameData.Territories, attackingRegion, defendingRegion);

            if (attackOutcome.DefendingArmy == DefendingArmy.IsEliminated)
            {
                var defendingPlayer = _gameData.Territories.Single(x => x.Region == defendingRegion).Player;
                DefendingArmyIsEliminated(defendingPlayer, attackingRegion, defendingRegion, attackOutcome.Territories);
            }
            else
            {
                MoveOnToAttackPhase(attackOutcome.Territories);
            }
        }

        private void DefendingArmyIsEliminated(Core.IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.AwardCardAtEndOfTurn;

            if (_attacker.IsGameOver(updatedTerritories))
            {
                GameIsOver(updatedTerritories);
            }
            else if (_attacker.IsPlayerEliminated(updatedTerritories, defeatedPlayer))
            {
                AquireAllCardsFromPlayerAndSendArmiesToOccupy(defeatedPlayer, attackingRegion, defeatedRegion, updatedTerritories);
            }
            else
            {
                SendArmiesToOccupy(updatedTerritories, attackingRegion, defeatedRegion);
            }
        }

        private void GameIsOver(IReadOnlyList<ITerritory> territories)
        {
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, territories, Deck);

            _gameStateConductor.GameIsOver(gameData);
        }

        private void AquireAllCardsFromPlayerAndSendArmiesToOccupy(Core.IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            var eliminatedPlayer = _gameData.Players.Single(player => player == defeatedPlayer);

            AquireAllCardsFromEliminatedPlayer(eliminatedPlayer);
            SendArmiesToOccupy(updatedTerritories, attackingRegion, defeatedRegion);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayer eliminatedPlayer)
        {
            var aquiredCards = eliminatedPlayer.AquireAllCards();
            foreach (var aquiredCard in aquiredCards)
            {
                _gameData.CurrentPlayer.AddCard(aquiredCard);
            }
        }

        private void SendArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion)
        {
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, territories, Deck);

            _gameStateConductor.SendArmiesToOccupy(gameData, sourceRegion, destinationRegion);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories)
        {
            var gameData = _gameDataFactory.Create(_gameData.CurrentPlayer, Players, updatedTerritories, Deck);

            _gameStateConductor.ContinueWithAttackPhase(gameData, _conqueringAchievement);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                return false;
            }

            return _fortifier.CanFortify(_gameData.Territories, sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!IsCurrentPlayerOccupyingRegion(sourceRegion))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(sourceRegion)}.");
            }

            var updatedTerritories = _fortifier.Fortify(_gameData.Territories, sourceRegion, destinationRegion, armies);

            var gameData = _gameDataFactory.Create(
                _gameData.CurrentPlayer,
                Players,
                updatedTerritories,
                Deck);

            _gameStateConductor.Fortify(gameData, sourceRegion, destinationRegion, armies);
        }

        public bool CanEndTurn()
        {
            return true;
        }

        public void EndTurn()
        {
            if (_conqueringAchievement == ConqueringAchievement.AwardCardAtEndOfTurn)
            {
                var card = Deck.Draw();
                _gameData.CurrentPlayer.AddCard(card);
            }

            _gameStateConductor.PassTurnToNextPlayer(this);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return false;
        }

        public int GetNumberOfArmiesToDraft()
        {
            return 0;
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
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
    }
}