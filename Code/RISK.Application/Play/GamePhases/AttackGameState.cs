using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class AttackGameState : GameStateBase
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IBattle _battle;
        private ConqueringAchievement _conqueringAchievement = ConqueringAchievement.DoNotAwardCardAtEndOfTurn;

        public AttackGameState(IGameStateConductor gameStateConductor, IGameDataFactory gameDataFactory, IBattle battle, GameData gameData)
            : base(gameData)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _battle = battle;
        }

        public override bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            var attackingTerritory = Territories.Single(x => x.Region == attackingRegion);
            var defendingTerritory = Territories.Single(x => x.Region == defendingRegion);

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

        public override IGameState Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!CanAttack(attackingRegion, defendingRegion))
            {
                throw new InvalidOperationException();
            }

            var attackingTerritory = GetTerritory(attackingRegion);
            var defendingTerritory = GetTerritory(defendingRegion);
            var defendingPlayer = defendingTerritory.Player;
            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            var updatedTerritories = Territories
                .Replace(attackingTerritory, battleResult.AttackingTerritory)
                .Replace(defendingTerritory, battleResult.DefendingTerritory)
                .ToList();

            if (battleResult.IsDefenderDefeated())
            {
                return DefenderIsDefeated(defendingPlayer, attackingRegion, defendingRegion, updatedTerritories);
            }

            return MoveOnToAttackPhase(updatedTerritories);
        }

        private IGameState DefenderIsDefeated(IPlayer defeatedPlayer, IRegion attackingRegion, IRegion defeatedRegion, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _conqueringAchievement = ConqueringAchievement.AwardCardAtEndOfTurn;

            if (IsGameOver(updatedTerritories))
            {
                return GameIsOver(updatedTerritories);
            }

            var isPlayerEliminated = IsPlayerEliminated(defeatedPlayer, updatedTerritories);
            if (isPlayerEliminated)
            {
                AquireAllCardsFromEliminatedPlayer(defeatedPlayer);
            }

            return SendArmiesToOccupy(updatedTerritories, attackingRegion, defeatedRegion);
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

        private IGameState GameIsOver(IReadOnlyList<ITerritory> territories)
        {
            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, territories, Deck);

            return _gameStateConductor.GameIsOver(gameData);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayer eliminatedPlayer)
        {
            var aquiredCards = eliminatedPlayer.AquireAllCards();
            foreach (var aquiredCard in aquiredCards)
            {
                CurrentPlayer.AddCard(aquiredCard);
            }
        }

        private IGameState SendArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion)
        {
            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, territories, Deck);

            return _gameStateConductor.SendArmiesToOccupy(gameData, sourceRegion, destinationRegion);
        }

        private IGameState MoveOnToAttackPhase(IReadOnlyList<ITerritory> updatedTerritories)
        {
            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, updatedTerritories, Deck);

            return _gameStateConductor.ContinueWithAttackPhase(gameData, _conqueringAchievement);
        }

        public override bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            var sourceTerritory = GetTerritory(sourceRegion);
            var destinationTerritory = GetTerritory(destinationRegion);
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

        public override IGameState Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!CanFortify(sourceRegion, destinationRegion))
            {
                throw new InvalidOperationException();
            }

            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, Territories, Deck);

            return _gameStateConductor.Fortify(gameData, sourceRegion, destinationRegion, armies);
        }

        public override bool CanEndTurn()
        {
            return true;
        }

        public override IGameState EndTurn()
        {
            if (_conqueringAchievement == ConqueringAchievement.AwardCardAtEndOfTurn)
            {
                var card = Deck.Draw();
                CurrentPlayer.AddCard(card);
            }

            return _gameStateConductor.PassTurnToNextPlayer(this);
        }
    }
}