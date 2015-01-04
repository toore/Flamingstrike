using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState(IStateController stateController, IPlayer player);
        IInteractionState CreateAttackState(IStateController stateController, IPlayer player, ITerritory selectedTerritory);
        IInteractionState CreateFortifyState(IStateController stateController, IPlayer player);
        IInteractionState CreateFortifyState(IStateController stateController, IPlayer player, ITerritory selectedTerritory);
    }
}