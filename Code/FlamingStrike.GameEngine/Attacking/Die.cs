using Toore.Shuffling;

namespace FlamingStrike.GameEngine.Attacking
{
    public interface IDie
    {
        int Roll();
    }

    public class Die : IDie
    {
        private readonly IRandomWrapper _randomWrapper;

        public Die(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public int Roll()
        {
            return _randomWrapper.Next(1, 7);
        }
    }
}