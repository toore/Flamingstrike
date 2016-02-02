using System;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;

namespace RISK.Application.Play.GamePhases
{
    public class AttackGameState : GameStateBase
    {
        private readonly GameStateFactory _gameStateFactory;
        private readonly IBattle _battle;
        private readonly INewArmiesDraftCalculator _newArmiesDraftCalculator;

        public AttackGameState(GameStateFactory gameStateFactory, GameData gameData, IBattle battle, INewArmiesDraftCalculator newArmiesDraftCalculator)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
            _battle = battle;
            _newArmiesDraftCalculator = newArmiesDraftCalculator;
        }

        public override bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            ThrowIfTerritoriesDoesNotContain(attackingTerritory);
            ThrowIfTerritoriesDoesNotContain(defendingTerritory);

            if (!IsCurrentPlayerAttacking(attackingTerritory))
            {
                return false;
            }

            var canAttack = HasBorder(attackingTerritory, defendingTerritory)
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

        public override IGameState Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            if (!CanAttack(attackingTerritory, defendingTerritory))
            {
                throw new InvalidOperationException();
            }

            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            // ... update territories here somewhere...
            var updatedTerritories = Territories;

            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories);

            if (battleResult.IsDefenderDefeated())
            {
                //_playerShouldReceiveCardWhenTurnEnds = true;
                return _gameStateFactory.CreateSendInArmiesToOccupyState(gameData);
            }

            return _gameStateFactory.CreateAttackState(gameData);
        }

        public override IGameState EndTurn()
        {
            //if (_playerShouldReceiveCardWhenTurnEnds)
            //{
            //    //PlayerId.AddCard(_cardFactory.Create());
            //}

            //_playerShouldReceiveCardWhenTurnEnds = false;
            var nextPlayer = GetNextPlayer();

            var gameData = new GameData(nextPlayer, Players, Territories);

            var numberOfArmiesToDraft = _newArmiesDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);

            return _gameStateFactory.CreateDraftArmiesGameState(gameData, numberOfArmiesToDraft);
        }

        private IPlayer GetNextPlayer()
        {
            return Players.ToList().GetNextOrFirst(CurrentPlayer);
        }
    }
}