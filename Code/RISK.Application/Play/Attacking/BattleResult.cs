﻿namespace RISK.Application.Play.Attacking
{
    public interface IBattleResult
    {
        bool IsDefenderDefeated();
        ITerritory AttackingTerritory { get; }
        ITerritory DefendingTerritory { get; }
    }

    public class BattleResult : IBattleResult
    {
        public ITerritory AttackingTerritory { get; }
        public ITerritory DefendingTerritory { get; }

        public BattleResult(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            AttackingTerritory = attackingTerritory;
            DefendingTerritory = defendingTerritory;
        }

        public bool IsDefenderDefeated()
        {
            return AttackingTerritory.Player == DefendingTerritory.Player;
        }
    }
}