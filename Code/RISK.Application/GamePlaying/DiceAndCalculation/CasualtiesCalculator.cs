using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class CasualtiesCalculator : ICasualtiesCalculator
    {
        public Casualties CalculateCasualties(IEnumerable<DiceValue> attackDices, IEnumerable<DiceValue> defendDices)
        {
            var matchedDices = MatchDices(attackDices,defendDices).ToList();

            return new Casualties
            {
                AttackerCasualties = GetAttackerCasualties(matchedDices),
                DefenderCasualties = GetDefenderCasualties(matchedDices)
            };
        }

        private static int GetAttackerCasualties(IEnumerable<DicePair> matchedDices)
        {
            return matchedDices.Count(x => HasDefenderWon(x.AttackerStrength, x.DefenderStrength));
        }

        private static int GetDefenderCasualties(IEnumerable<DicePair> matchedDices)
        {
            return matchedDices.Count(x => HasAttackerWon(x.AttackerStrength, x.DefenderStrength));
        }

        private static IEnumerable<DicePair> MatchDices(IEnumerable<DiceValue> attackDices, IEnumerable<DiceValue> defendDices)
        {
            var attackDicesClosure = attackDices.ToList();
            var defendDicesClosure = defendDices.ToList();

            var usedDices = Math.Min(attackDicesClosure.Count, defendDicesClosure.Count);

            var defendersValuesUsed = defendDicesClosure
                .OrderByDescending(x => x)
                .Take(usedDices);

            var defendersValuesStack = new Stack<DiceValue>(defendersValuesUsed);

            return attackDicesClosure
                .OrderByDescending(x => x)
                .Take(usedDices)
                .Select(x => new DicePair(x, defendersValuesStack.Pop()));
        }

        private static bool HasDefenderWon(DiceValue attacker, DiceValue defender)
        {
            return defender >= attacker;
        }

        private static bool HasAttackerWon(DiceValue attacker, DiceValue defender)
        {
            return attacker > defender;
        }

        private class DicePair
        {
            public DicePair(DiceValue attackerStrength, DiceValue defenderStrength)
            {
                AttackerStrength = attackerStrength;
                DefenderStrength = defenderStrength;
            }

            public DiceValue AttackerStrength { get; private set; }
            public DiceValue DefenderStrength { get; private set; }
        }
    }
}