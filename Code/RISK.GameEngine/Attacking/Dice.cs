using Toore.Shuffling;

namespace RISK.Core
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