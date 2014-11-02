using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState(StateController stateController, IPlayer player, IWorldMap worldMap);
        IInteractionState CreateAttackState(StateController stateController, IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory);
    }
}