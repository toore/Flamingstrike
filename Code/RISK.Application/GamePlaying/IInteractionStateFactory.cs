using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState(StateController stateController, IPlayer player);
        IInteractionState CreateAttackState(StateController stateController, IPlayer player, ITerritory selectedTerritory);
        IInteractionState CreateFortifyState(StateController stateController, IPlayer player);
        IInteractionState CreateFortifyState(StateController stateController, IPlayer player, ITerritory selectedTerritory);
    }
}