using System;
using RISK.Application.World;

namespace RISK.Application.Play.Attacking
{
    public interface IBattle
    {
        BattleResult Attack(ITerritory attacker, ITerritory defender);
    }

    public enum BattleResult
    {
        NoChange,
        DefenderEliminated
    }

    public class Battle : IBattle
    {
        private readonly IDicesRoller _dicesRoller;
        private readonly IBattleCalculator _battleCalculator;

        public Battle(IDicesRoller dicesRoller, IBattleCalculator battleCalculator)
        {
            _dicesRoller = dicesRoller;
            _battleCalculator = battleCalculator;
        }

        public BattleResult Attack(ITerritory attacker, ITerritory defender)
        {
            throw new NotImplementedException();
        }

        //public IGameboard Attack(IGameboard gameboard, IGameboardTerritory attacker, IGameboardTerritory defender)
        //{
        //    var attackingArmies = Math.Min(attacker.GetNumberOfAttackingArmies(), 3);
        //    var defendingArmies = Math.Min(defender.GetNumberOfDefendingArmies(), 2);

        //    var dices = _dicesRoller.Roll(attackingArmies, defendingArmies);

        //    var battleResult = _battleCalculator.Battle(dices.AttackValues, dices.DefenceValues);

            //attacker.Armies -= battleResult.AttackerLosses;
            //defender.Armies -= battleResult.DefenderLosses;

        //    //if (IsDefenderDefeated(defender))
        //    //{
        //    //    OccupyTerritory(attacker, defender);
        //    //}
        //    return null;//remove
        //}

        //private static void OccupyTerritory(ITerritoryGeography attacker, ITerritoryGeography defender)
        //{
        //    //const int armiesLeftBehind = 1;

        //    //defender.Occupant = attacker.Occupant;
        //    //defender.Armies = attacker.Armies - armiesLeftBehind;
        //    //attacker.Armies = armiesLeftBehind;
        //}

        //private static bool IsDefenderDefeated(ITerritoryGeography defender)
        //{
        //    //return defender.Armies == 0;
        //    return false;
        //}
    }
}