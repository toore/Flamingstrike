using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play.Attacking
{
    public interface IBattleOutcomeCalculator
    {
        BattleOutcome Battle(IEnumerable<int> attack, IEnumerable<int> defence);
    }

    public class BattleOutcome
    {
        public BattleOutcome(int attackerLosses, int defenderLosses)
        {
            AttackerLosses = attackerLosses;
            DefenderLosses = defenderLosses;
        }

        public int AttackerLosses { get; }
        public int DefenderLosses { get; }
    }

    public class BattleOutcomeCalculator : IBattleOutcomeCalculator
    {
        public BattleOutcome Battle(IEnumerable<int> attack, IEnumerable<int> defence)
        {
            var matchedAttackAndDefenceValues = MatchAttackAndDefenceValues(attack.ToList(), defence.ToList())
                .ToList();

            return new BattleOutcome(
                GetAttackerLosses(matchedAttackAndDefenceValues),
                GetDefenderLosses(matchedAttackAndDefenceValues));
        }

        private static int GetAttackerLosses(IEnumerable<AttackAndDefenceMatch> attackAndDefenceMatches)
        {
            return attackAndDefenceMatches.Count(x => HasDefenderWon(x.Attack, x.Defence));
        }

        private static int GetDefenderLosses(IEnumerable<AttackAndDefenceMatch> attackAndDefenceMatches)
        {
            return attackAndDefenceMatches.Count(x => HasAttackerWon(x.Attack, x.Defence));
        }

        private static IEnumerable<AttackAndDefenceMatch> MatchAttackAndDefenceValues(ICollection<int> attackValues, ICollection<int> defenceValues)
        {
            var numberOfUsedValues = Math.Min(attackValues.Count, defenceValues.Count);

            var defendersValuesUsed = defenceValues
                .OrderByDescending(x => x)
                .Take(numberOfUsedValues);

            var defendersValuesStack = new Stack<int>(defendersValuesUsed);

            var matchedAttackAndDefenceValues = attackValues
                .OrderByDescending(x => x)
                .Take(numberOfUsedValues)
                .Select(x => new AttackAndDefenceMatch(x, defendersValuesStack.Pop()));

            return matchedAttackAndDefenceValues;
        }

        private static bool HasDefenderWon(int attack, int defence)
        {
            return defence >= attack;
        }

        private static bool HasAttackerWon(int attack, int defence)
        {
            return attack > defence;
        }

        private class AttackAndDefenceMatch
        {
            public AttackAndDefenceMatch(int attack, int defence)
            {
                Attack = attack;
                Defence = defence;
            }

            public int Attack { get; }
            public int Defence { get; }
        }
    }
}