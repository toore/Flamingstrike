using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class Dices : IDices
    {
        private readonly ICasualtyEvaluator _casualtyEvaluator;
        private readonly IDiceRoller _diceRoller;

        public Dices(ICasualtyEvaluator casualtyEvaluator, IDiceRoller diceRoller)
        {
            _casualtyEvaluator = casualtyEvaluator;
            _diceRoller = diceRoller;
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
                .Select(x => _diceRoller.Roll())
                .ToList();
        }
    }
}