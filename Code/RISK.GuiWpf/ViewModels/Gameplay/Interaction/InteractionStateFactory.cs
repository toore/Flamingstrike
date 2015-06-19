using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState(IStateController stateController, IPlayer player);
        IInteractionState CreateAttackState(IStateController stateController, IPlayer player, ITerritory selectedTerritory);
        IInteractionState CreateFortifyState(IStateController stateController, IPlayer player);
        IInteractionState CreateFortifyState(IStateController stateController, IPlayer player, ITerritory selectedTerritory);
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        public IInteractionState CreateSelectState(IStateController stateController, IPlayer player)
        {
            return new SelectState(stateController, this, player);
        }

        public IInteractionState CreateAttackState(IStateController stateController, IPlayer player, ITerritory selectedTerritory)
        {
            return new AttackState(stateController, this, player, selectedTerritory);
        }

        public IInteractionState CreateFortifyState(IStateController stateController, IPlayer player)
        {
            return new FortifySelectState(stateController, this, player);
        }

        public IInteractionState CreateFortifyState(IStateController stateController, IPlayer player, ITerritory selectedTerritory)
        {
            return new FortifyMoveState(player, selectedTerritory);
        }
    }
}