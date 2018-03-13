using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public class DiceResult
    {
        public DiceResult(IList<int> attackValues, IList<int> defenceValues)
        {
            DefenceValues = defenceValues;
            AttackValues = attackValues;
        }

        public IList<int> AttackValues { get; }
        public IList<int> DefenceValues { get; }
    }

    public interface IDice
    {
        DiceResult Roll(int numberOfAttackDices, int numberOfDefenceDices);
    }

    public class Dice : IDice
    {
        private readonly IDie _die;

        public Dice(IDie die)
        {
            _die = die;
        }

        public DiceResult Roll(int numberOfAttackDices, int numberOfDefenceDices)
        {
            var attackValues = RollDices(numberOfAttackDices);
            var defenceValues = RollDices(numberOfDefenceDices);

            return new DiceResult(attackValues, defenceValues);
        }

        private IList<int> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _die.Roll())
                .ToList();
        }
    }
}