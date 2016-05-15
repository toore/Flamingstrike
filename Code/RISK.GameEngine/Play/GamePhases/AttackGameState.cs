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
        private readonly IBattle _battle;
        private readonly GameData _gameData;
        private ConqueringAchievement _conqueringAchievement = ConqueringAchievement.DoNotAwardCardAtEndOfTurn;

        public AttackGameState(IGameStateConductor gameStateConductor, IGameDataFactory gameDataFactory, IBattle battle, GameData gameData)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _battle = battle;
            _gameData = gameData;
        }

        public IPlayer CurrentPlayer => _gameData.CurrentPlayer;
        public IReadOnlyList<IPlayer> Players => _gameData.Players;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public IDeck Deck => _gameData.Deck;

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            var attackingTerritory = _gameData.Territories.Single(x => x.Region == attackingRegion);
            var defendingTerritory = _gameData.Territories.Single(x => x.Region == defendingRegion);

            if (!IsCurrentPlayerAttacking(attackingTerritory))
            {
                return false;
            }

            var canAttack =
                HasBorder(attackingTerritory, defendingTerritory)
                &&
                IsAttackerAndDefenderDifferentPlayers(attackingTerritory, defendingTerritory)
                &&
                HasAttackerEnoughArmiesToPerformAttack(attackingTerritory);

            return canAttack;
        }

        private bool IsCurrentPlayerAttacking(ITerritory attackingTerritory)
        {
            return CurrentPlayer == attackingTerritory.Player;
        }

        private static bool HasBorder(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Region.HasBorder(defendingTerritory.Region);
        }

        private static bool IsAttackerAndDefenderDifferentPlayers(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Player != defendingTerritory.Player;
        }

        private static bool HasAttackerEnoughArmiesToPerformAttack(ITerritory attackingTerritory)
        {
            return attackingTerritory.GetNumberOfArmiesAvailableForAttack() > 0;
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!CanAttack(attackingRegion, defendingRegion))
            {
                throw new InvalidOperationException();
            }

            var attackingTerritory = _gameData.Territories.GetTerritory(attackingRegion);
            var defendingTerritory = _gameData.Territories.GetTerritory(defendingRegion);
            var defendingPlayer = defendingTerritory.Player;
            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            var updatedTerritories = _gameData.Territories
                .Replace(attackingTerritory, battleResult.UpdatedAttackingTerritory)
                .Replace(defendingTerritory, battleResult.UpdatedDefendingTerritory)
                .ToList();

            if (battleResult.IsDefenderDefeated())
            {
                DefenderIsDefeated(defendingPlayer, attackingRegion, defendingRegion, updatedTerritories);
            }
            else
            {
                MoveOnToAttackPhase(updatedTerritories);
            }
        }

        private void DefenderIsDefeated(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.AwardCardAtEndOfTurn;

            if (IsGameOver(updatedTerritories))
            {
                GameIsOver(updatedTerritories);
            }
            else
            {
                var isPlayerEliminated = IsPlayerEliminated(defeatedPlayer, updatedTerritories);
                if (isPlayerEliminated)
                {
                    AquireAllCardsFromEliminatedPlayer(defeatedPlayer);
                }

                SendArmiesToOccupy(updatedTerritories, attackingRegion, defeatedRegion);
            }
        }

        private static bool IsPlayerEliminated(IPlayer player, IEnumerable<ITerritory> territories)
        {
            var playerOccupiesTerritories = territories.Any(x => x.Player == player);

            return !playerOccupiesTerritories;
        }

        private static bool IsGameOver(IEnumerable<ITerritory> territories)
        {
            var allTerritoriesAreOccupiedBySamePlayer = territories
                .Select(x => x.Player)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }

        private void GameIsOver(IReadOnlyList<ITerritory> territories)
        {
            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, territories, Deck);

            _gameStateConductor.GameIsOver(gameData);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayer eliminatedPlayer)
        {
            var aquiredCards = eliminatedPlayer.AquireAllCards();
            foreach (var aquiredCard in aquiredCards)
            {
                CurrentPlayer.AddCard(aquiredCard);
            }
        }

        private void SendArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion)
        {
            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, territories, Deck);

            _gameStateConductor.SendArmiesToOccupy(gameData, sourceRegion, destinationRegion);
        }

        private void MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories)
        {
            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, updatedTerritories, Deck);

            _gameStateConductor.ContinueWithAttackPhase(gameData, _conqueringAchievement);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            var sourceTerritory = _gameData.Territories.GetTerritory(sourceRegion);
            var destinationTerritory = _gameData.Territories.GetTerritory(destinationRegion);
            var playerOccupiesBothTerritories =
                sourceTerritory.Player == CurrentPlayer
                &&
                sourceTerritory.Player == destinationTerritory.Player;
            var hasBorder = sourceRegion.HasBorder(destinationRegion);

            var canFortify =
                playerOccupiesBothTerritories
                &&
                hasBorder;

            return canFortify;
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!CanFortify(sourceRegion, destinationRegion))
            {
                throw new InvalidOperationException();
            }

            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, _gameData.Territories, Deck);

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
                CurrentPlayer.AddCard(card);
            }

            _gameStateConductor.PassTurnToNextPlayer(this);
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