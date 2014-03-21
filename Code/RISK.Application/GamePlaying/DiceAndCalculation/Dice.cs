namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class Dice : IDice
    {
        private readonly IRandomWrapper _randomWrapper;

        public Dice(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public DiceValue Roll()
        {
            return (DiceValue)(_randomWrapper.Next(6) + 1);
        }
    }
}