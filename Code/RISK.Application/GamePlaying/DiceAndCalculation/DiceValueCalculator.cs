using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class DiceValueCalculator : IDiceValueCalculator
    {
        public int CalculateAttackerCasualties(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            return MatchDices(attacker, defender)
                .Count(x => HasAttackerLost(x.Attack, x.Defend));
        }

        public int CalculateDefenderCasualties(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            return MatchDices(attacker, defender)
                .Count(x => HasDefenderLost(x.Attack, x.Defend));
        }

        private static IEnumerable<DicePair> MatchDices(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            var attackerClosure = attacker.ToList();
            var defenderClosure = defender.ToList();

            var usedDices = Math.Min(attackerClosure.Count, defenderClosure.Count);

            var defendersValuesUsed = defenderClosure
                .OrderByDescending(x => x)
                .Take(usedDices);

            var defendersValuesStack = new Stack<DiceValue>(defendersValuesUsed);

            return attackerClosure
                .OrderByDescending(x => x)
                .Take(usedDices)
                .Select(x => new DicePair(x, defendersValuesStack.Pop()));
        }

        private static bool HasAttackerLost(DiceValue attacker, DiceValue defender)
        {
            return attacker <= defender;
        }

        private static bool HasDefenderLost(DiceValue attacker, DiceValue defender)
        {
            return !HasAttackerLost(attacker, defender);
        }

        public class DicePair
        {
            public DicePair(DiceValue attack, DiceValue defend)
            {
                Attack = attack;
                Defend = defend;
            }

            public DiceValue Attack { get; private set; }
            public DiceValue Defend { get; private set; }
        }
    }
}