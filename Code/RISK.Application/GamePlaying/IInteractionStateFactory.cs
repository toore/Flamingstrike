using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState(IPlayer player, IWorldMap worldMap);
        IInteractionState CreateAttackState(IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory);
        IInteractionState CreateFortificationState(IPlayer player, IWorldMap worldMap);
    }
}