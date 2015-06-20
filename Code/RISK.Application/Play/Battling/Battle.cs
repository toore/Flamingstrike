using System;
using RISK.Application.World;

namespace RISK.Application.Play.Battling
{
    public interface IBattle
    {
        IGameboard Attack(IGameboard gameboard, IGameboardTerritory attacker, IGameboardTerritory defender);
    }

    public class Battle : IBattle
    {
        private readonly IDiceRoller _diceRoller;
        private readonly IBattleCalculator _battleCalculator;

        public Battle(IDiceRoller diceRoller, IBattleCalculator battleCalculator)
        {
            _diceRoller = diceRoller;
            _battleCalculator = battleCalculator;
        }

        public IGameboard Attack(IGameboard gameboard, IGameboardTerritory attacker, IGameboardTerritory defender)
        {
            var attackingArmies = Math.Min(attacker.GetNumberOfAttackingArmies(), 3);
            var defendingArmies = Math.Min(defender.GetNumberOfDefendingArmies(), 2);

            var dices = _diceRoller.Roll(attackingArmies, defendingArmies);

            var battleResult = _battleCalculator.Battle(dices.AttackValues, dices.DefenceValues);

            //attacker.Armies -= battleResult.AttackerLosses;
            //defender.Armies -= battleResult.DefenderLosses;

            //if (IsDefenderDefeated(defender))
            //{
            //    OccupyTerritory(attacker, defender);
            //}
            return null;//remove
        }

        private static void OccupyTerritory(ITerritory attacker, ITerritory defender)
        {
            const int armiesLeftBehind = 1;

            //defender.Occupant = attacker.Occupant;
            //defender.Armies = attacker.Armies - armiesLeftBehind;
            //attacker.Armies = armiesLeftBehind;
        }

        private static bool IsDefenderDefeated(ITerritory defender)
        {
            //return defender.Armies == 0;
            return false;
        }
    }
}