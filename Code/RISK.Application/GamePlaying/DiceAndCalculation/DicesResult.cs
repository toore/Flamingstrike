namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public interface IDicesResult
    {
        int AttackerCasualties { get; }
        int DefenderCasualties { get; }
    }

    public class DicesResult : IDicesResult
    {
        public int AttackerCasualties { get; set; }
        public int DefenderCasualties { get; set; }
    }
}