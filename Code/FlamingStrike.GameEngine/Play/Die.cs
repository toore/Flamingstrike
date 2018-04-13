using Toore.Shuffling;

namespace FlamingStrike.GameEngine.Play
{
    public interface IDie
    {
        int Roll();
    }

    public class Die : IDie
    {
        private const int Max = 6;

        private readonly IRandomWrapper _randomWrapper;

        public Die(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public int Roll()
        {
            const int minValueInclusive = 1;
            const int maxValueExclusive = Max + 1;
            return _randomWrapper.Next(minValueInclusive, maxValueExclusive);
        }
    }
}