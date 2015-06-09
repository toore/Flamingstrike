using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public interface ICasualtiesCalculator
    {
        Casualties CalculateCasualties(IEnumerable<int> attack, IEnumerable<int> defence);
    }

    public class Casualties
    {
        public int AttackerCasualties { get; set; }
        public int DefenderCasualties { get; set; }
    }

    public class CasualtiesCalculator : ICasualtiesCalculator
    {
        public Casualties CalculateCasualties(IEnumerable<int> attack, IEnumerable<int> defence)
        {
            var matchedAttackAndDefenceValues = MatchAttackAndDefenceValues(attack.ToList(), defence.ToList())
                .ToList();

            return new Casualties
            {
                AttackerCasualties = GetAttackerCasualties(matchedAttackAndDefenceValues),
                DefenderCasualties = GetDefenderCasualties(matchedAttackAndDefenceValues)
            };
        }

        private static int GetAttackerCasualties(IEnumerable<AttackAndDefenceMatch> attackAndDefenceMatches)
        {
            return attackAndDefenceMatches.Count(x => HasDefenderWon(x.Attack, x.Defence));
        }

        private static int GetDefenderCasualties(IEnumerable<AttackAndDefenceMatch> attackAndDefenceMatches)
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