namespace RISK.Application.GamePlaying
{
    public interface IBattleCalculator
    {
        void Attack(ITerritory attacker, ITerritory defender);
    }
}