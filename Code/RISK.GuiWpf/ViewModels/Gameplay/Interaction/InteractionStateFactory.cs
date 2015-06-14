using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState(IStateController stateController, IPlayerId playerId);
        IInteractionState CreateAttackState(IStateController stateController, IPlayerId playerId, ITerritory selectedTerritory);
        IInteractionState CreateFortifyState(IStateController stateController, IPlayerId playerId);
        IInteractionState CreateFortifyState(IStateController stateController, IPlayerId playerId, ITerritory selectedTerritory);
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        public IInteractionState CreateSelectState(IStateController stateController, IPlayerId playerId)
        {
            return new SelectState(stateController, this, playerId);
        }

        public IInteractionState CreateAttackState(IStateController stateController, IPlayerId playerId, ITerritory selectedTerritory)
        {
            return new AttackState(stateController, this, playerId, selectedTerritory);
        }

        public IInteractionState CreateFortifyState(IStateController stateController, IPlayerId playerId)
        {
            return new FortifySelectState(stateController, this, playerId);
        }

        public IInteractionState CreateFortifyState(IStateController stateController, IPlayerId playerId, ITerritory selectedTerritory)
        {
            return new FortifyMoveState(playerId, selectedTerritory);
        }
    }
}