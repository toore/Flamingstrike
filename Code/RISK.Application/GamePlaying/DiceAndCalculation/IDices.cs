namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public interface IDices
    {
        IDicesResult Roll(int attackingArmies, int defendingArmies);
    }
}