using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IBattleEvaluater
    {
        void Attack(IArea attacker, IArea defender);
    }
}