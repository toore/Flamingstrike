using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class Dices : IDices
    {
        private readonly ICasualtiesCalculator _casualtiesCalculator;
        private readonly IDice _dice;

        public Dices(ICasualtiesCalculator casualtiesCalculator, IDice dice)
        {
            _casualtiesCalculator = casualtiesCalculator;
            _dice = dice;
        }

        public IDicesResult Roll(int attackingArmies, int defendingArmies)
        {
            var noOfAttackingDices = Math.Min(attackingArmies, 3);
            var noOfDefendingDices = Math.Min(defendingArmies, 2);

            var attackDices = RollDices(noOfAttackingDices).ToList();
            var defendDices = RollDices(noOfDefendingDices).ToList();

            var casualties = _casualtiesCalculator.CalculateCasualties(attackDices, defendDices);

            return new DicesResult
            {
                AttackDices = attackDices,
                DefendDices = defendDices,
                AttackerCasualties = casualties.AttackerCasualties,
                DefenderCasualties = casualties.DefenderCasualties
            };
        }

        private IEnumerable<DiceValue> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _dice.Roll())
                .ToList();
        }
    }
}