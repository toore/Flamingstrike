namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public interface IDices
    {
        IDicesResult Roll(int attackingArmies, int defendingArmies);
    }
}