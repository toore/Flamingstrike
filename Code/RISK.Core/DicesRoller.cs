using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface IDicesRoller
    {
        Dices Roll(int numberOfAttackDices, int numberOfDefenceDices);
    }

    public class Dices
    {
        public Dices(IList<int> attackValues, IList<int> defenceValues)
        {
            DefenceValues = defenceValues;
            AttackValues = attackValues;
        }

        public IList<int> AttackValues { get; }
        public IList<int> DefenceValues { get; }
    }

    public class DicesRoller : IDicesRoller
    {
        private readonly IDice _dice;

        public DicesRoller(IDice dice)
        {
            _dice = dice;
        }

        public Dices Roll(int numberOfAttackDices, int numberOfDefenceDices)
        {
            var attackValues = RollDices(numberOfAttackDices);
            var defenceValues = RollDices(numberOfDefenceDices);

            return new Dices(attackValues, defenceValues);
        }

        private IList<int> RollDices(int numberOfDices)
        {
            return Enumerable.Range(0, numberOfDices)
                .Select(x => _dice.Roll())
                .ToList();
        }
    }
}