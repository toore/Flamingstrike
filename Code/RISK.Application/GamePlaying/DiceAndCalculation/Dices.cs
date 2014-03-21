using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class Dices : IDices
    {
        private readonly ICasualtyEvaluator _casualtyEvaluator;
        private readonly IDice _dice;

        public Dices(ICasualtyEvaluator casualtyEvaluator, IDice dice)
        {
            _casualtyEvaluator = casualtyEvaluator;
            _dice = dice;
        }

        public IDicesResult Roll(int attackingArmies, int defendingArmies)
        {
            var attackingDices = Math.Min(attackingArmies, 3);
            var defendingDices = Math.Min(defendingArmies, 2);

            return new DicesResult(_casualtyEvaluator, RollDices(attackingDices), RollDices(defendingDices));
        }

        private IEnumerable<DiceValue> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _dice.Roll())
                .ToList();
        }
    }
}