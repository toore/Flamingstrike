using RISK.GameEngine.Shuffling;

namespace RISK.GameEngine.Attacking
{
    public interface IDice
    {
        int Roll();
    }

    public class Dice : IDice
    {
        private readonly IRandomWrapper _randomWrapper;

        public Dice(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public int Roll()
        {
            return _randomWrapper.Next(1, 7);
        }
    }
}