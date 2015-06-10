using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public interface IDices
    {
        IDicesResult Roll(int numberOfAttackDices, int numberOfDefenceDices);
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

        public IDicesResult Roll(int numberOfAttackDices, int numberOfDefenceDices)
        {
            var attackValues = RollDices(numberOfAttackDices);
            var defenceValues = RollDices(numberOfDefenceDices);

            var casualties = _casualtiesCalculator.CalculateCasualties(attackValues, defenceValues);

            return new DicesResult
            {
                AttackerCasualties = casualties.AttackerCasualties,
                DefenderCasualties = casualties.DefenderCasualties
            };
        }

        private IEnumerable<int> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _dice.Roll())
                .ToList();
        }
    }
}