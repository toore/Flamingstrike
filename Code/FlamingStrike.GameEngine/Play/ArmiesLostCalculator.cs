using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IArmiesLostCalculator
    {
        int CalculateAttackerLosses(ICollection<int> attack, ICollection<int> defence);
        int CalculateDefenderLosses(ICollection<int> attack, ICollection<int> defence);
    }

    public class ArmiesLostCalculator : IArmiesLostCalculator
    {
        public int CalculateAttackerLosses(ICollection<int> attack, ICollection<int> defence)
        {
            var matchedAttackAndDefenceValues = MatchAttackAndDefenceValues(attack, defence);

            return GetAttackerLosses(matchedAttackAndDefenceValues);
        }

        public int CalculateDefenderLosses(ICollection<int> attack, ICollection<int> defence)
        {
            var matchedAttackAndDefenceValues = MatchAttackAndDefenceValues(attack, defence);

            return GetDefenderLosses(matchedAttackAndDefenceValues);
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

        private static int GetAttackerLosses(IEnumerable<AttackAndDefenceMatch> attackAndDefenceMatches)
        {
            return attackAndDefenceMatches.Count(x => HasDefenderWon(x.Attack, x.Defence));
        }

        private static int GetDefenderLosses(IEnumerable<AttackAndDefenceMatch> attackAndDefenceMatches)
        {
            return attackAndDefenceMatches.Count(x => HasAttackerWon(x.Attack, x.Defence));
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