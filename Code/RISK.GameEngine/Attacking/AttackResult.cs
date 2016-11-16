namespace RISK.GameEngine.Attacking
{
    public interface IAttackResult
    {
        bool IsDefenderDefeated();
        ITerritory UpdatedAttackingTerritory { get; }
        ITerritory UpdatedDefendingTerritory { get; }
    }

    public class AttackResult : IAttackResult
    {
        public ITerritory UpdatedAttackingTerritory { get; }
        public ITerritory UpdatedDefendingTerritory { get; }

        public AttackResult(ITerritory updatedAttackingTerritory, ITerritory updatedDefendingTerritory)
        {
            UpdatedAttackingTerritory = updatedAttackingTerritory;
            UpdatedDefendingTerritory = updatedDefendingTerritory;
        }

        public bool IsDefenderDefeated()
        {
            return UpdatedAttackingTerritory.Player == UpdatedDefendingTerritory.Player;
        }
    }
}