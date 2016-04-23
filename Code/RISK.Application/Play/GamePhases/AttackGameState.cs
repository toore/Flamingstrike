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
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IBattle _battle;
        private bool _playerShouldBeAwardedCardWhenTurnEnds;

        public AttackGameState(IGameStateFactory gameStateFactory, IBattle battle, GameData gameData)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
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
                .Update(attackingTerritory, battleResult.AttackingTerritory)
                .Update(defendingTerritory, battleResult.DefendingTerritory)
                .ToList();

            if (battleResult.IsDefenderDefeated())
            {
                return CreateSendInArmiesToOccupyState(defendingPlayer, updatedTerritories);
            }

            return CreateAttackState(updatedTerritories);
        }

        private IGameState CreateSendInArmiesToOccupyState(IPlayer defeatedPlayer, IReadOnlyList<ITerritory> updatedTerritories)
        {
            _playerShouldBeAwardedCardWhenTurnEnds = true;

            var isPlayerEliminated = IsPlayerEliminated(defeatedPlayer, updatedTerritories);
            if (isPlayerEliminated)
            {
                AquireAllCardsFromEliminatedPlayer(defeatedPlayer);
            }

            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories, Deck);

            return _gameStateFactory.CreateSendInArmiesToOccupyGameState(gameData);
        }

        private void AquireAllCardsFromEliminatedPlayer(IPlayer eliminatedPlayer)
        {
            var aquiredCards = eliminatedPlayer.AquireAllCards();
            foreach (var aquiredCard in aquiredCards)
            {
                CurrentPlayer.AddCard(aquiredCard);
            }
        }

        private IGameState CreateAttackState(IReadOnlyList<ITerritory> updatedTerritories)
        {
            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories, Deck);

            return _gameStateFactory.CreateAttackGameState(gameData);
        }

        private static bool IsPlayerEliminated(IPlayer player, IEnumerable<ITerritory> territories)
        {
            var playerOccupiesTerritories = territories.Any(x => x.Player == player);

            return !playerOccupiesTerritories;
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

            var gameData = new GameData(CurrentPlayer, Players, Territories, Deck);

            return _gameStateFactory.CreateFortifyState(gameData, sourceRegion, destinationRegion, armies);
        }

        public override bool CanEndTurn()
        {
            return true;
        }

        public override IGameState EndTurn()
        {
            if (_playerShouldBeAwardedCardWhenTurnEnds)
            {
                var card = Deck.Draw();
                CurrentPlayer.AddCard(card);
            }

            return _gameStateFactory.CreateNextTurnGameState(this);
        }

        public bool IsGameOver()
        {
            var allTerritoriesAreOccupiedBySamePlayer = Territories
                .Select(x => x.Player)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }
    }
}