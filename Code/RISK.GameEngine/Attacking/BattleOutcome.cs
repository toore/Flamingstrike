namespace RISK.GameEngine.Attacking
{
    public interface IBattleOutcome
    {
        bool IsDefenderDefeated();
        ITerritory UpdatedAttackingTerritory { get; }
        ITerritory UpdatedDefendingTerritory { get; }
    }

    public class BattleOutcome : IBattleOutcome
    {
        public ITerritory UpdatedAttackingTerritory { get; }
        public ITerritory UpdatedDefendingTerritory { get; }

        public BattleOutcome(ITerritory updatedAttackingTerritory, ITerritory updatedDefendingTerritory)
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