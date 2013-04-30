namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class DiceRoller : IDiceRoller
    {
        private readonly IRandomWrapper _randomWrapper;

        public DiceRoller(IRandomWrapper randomWrapper)
        {
            _randomWrapper = randomWrapper;
        }

        public DiceValue Roll()
        {
            return (DiceValue)(_randomWrapper.Next(6) + 1);
        }
    }
}