using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IBattleCalculator
    {
        void Attack(ITerritory attacker, ITerritory defender);
    }
}