using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Attacking
{
    public interface IDiceRoller
    {
        Dice Roll(int numberOfAttackDices, int numberOfDefenceDices);
    }

    public class DiceRoller : IDiceRoller
    {
        private readonly IDie _die;

        public DiceRoller(IDie die)
        {
            _die = die;
        }

        public Dice Roll(int numberOfAttackDices, int numberOfDefenceDices)
        {
            var attackValues = RollDices(numberOfAttackDices);
            var defenceValues = RollDices(numberOfDefenceDices);

            return new Dice(attackValues, defenceValues);
        }

        private IList<int> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _die.Roll())
                .ToList();
        }
    }
}