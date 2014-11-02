namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public interface IDicesResult
    {
        int AttackerCasualties { get; }
        int DefenderCasualties { get; }
    }
}