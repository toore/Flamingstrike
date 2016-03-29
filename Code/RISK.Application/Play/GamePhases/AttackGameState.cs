using System;
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
            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            var updatedTerritories = Territories
                .Update(attackingTerritory, battleResult.AttackingTerritory)
                .Update(defendingTerritory, battleResult.DefendingTerritory)
                .ToList();

            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories);

            if (battleResult.IsDefenderDefeated())
            {
                //_playerShouldReceiveCardWhenTurnEnds = true;
                return _gameStateFactory.CreateSendInArmiesToOccupyGameState(gameData);
            }

            return _gameStateFactory.CreateAttackGameState(gameData);
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

            var gameData = new GameData(CurrentPlayer, Players, Territories);

            return _gameStateFactory.CreateFortifyState(gameData, sourceRegion, destinationRegion, armies);
        }

        public override bool CanEndTurn()
        {
            return true;
        }

        public override IGameState EndTurn()
        {
            //if (_playerShouldReceiveCardWhenTurnEnds)
            //{
            //    //PlayerId.AddCard(_cardFactory.Create());
            //}

            //_playerShouldReceiveCardWhenTurnEnds = false;
            IPlayer nextPlayer = null;//Players.

            var gameData = new GameData(nextPlayer, Players, Territories);

            return _gameStateFactory.CreateDraftArmiesGameState(gameData);
        }

        //private IPlayer GetNextPlayer()
        //{
        //    return Players.ToList().GetNextOrFirst(CurrentPlayer);
        //}

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