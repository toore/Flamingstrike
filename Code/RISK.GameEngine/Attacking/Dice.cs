using System.Collections.Generic;

namespace RISK.GameEngine.Attacking
{
    public class Dice
    {
        public Dice(IList<int> attackValues, IList<int> defenceValues)
        {
            DefenceValues = defenceValues;
            AttackValues = attackValues;
        }

        public IList<int> AttackValues { get; }
        public IList<int> DefenceValues { get; }
    }
}