using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public interface IDices
    {
        IDicesResult Roll(int attackingArmies, int defendingArmies);
    }

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
            var numberOfAttackingDices = Math.Min(attackingArmies, 3);
            var numberOfDefendingDices = Math.Min(defendingArmies, 2);

            var attackDices = RollDices(numberOfAttackingDices);
            var defendDices = RollDices(numberOfDefendingDices);

            var casualties = _casualtiesCalculator.CalculateCasualties(attackDices, defendDices);

            return new DicesResult
            {
                AttackerCasualties = casualties.AttackerCasualties,
                DefenderCasualties = casualties.DefenderCasualties
            };
        }

        private IList<int> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _dice.Roll())
                .ToList();
        }
    }
}