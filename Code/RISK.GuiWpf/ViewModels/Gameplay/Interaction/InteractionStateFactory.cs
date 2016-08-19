using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesInteractionState(IGame game);
        IInteractionState CreateSelectInteractionState(IGame game);
        IInteractionState CreateAttackInteractionState(IGame game, IRegion selectedRegion);
        IInteractionState CreateSendArmiesToOccupyInteractionState(IGame game, IRegion selectedRegion, IRegion conqueredRegion);
        IInteractionState CreateFortifySelectInteractionState(IGame game);
        IInteractionState CreateFortifyMoveInteractionState(IGame game, IRegion selectedRegion);
        IInteractionState CreateEndTurnInteractionState();
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        private readonly IInteractionContext _interactionContext;

        public InteractionStateFactory(IInteractionContext interactionContext)
        {
            _interactionContext = interactionContext;
        }

        public IInteractionState CreateDraftArmiesInteractionState(IGame game)
        {
            return new DraftArmiesInteractionState(_interactionContext, this, game);
        }

        public IInteractionState CreateSelectInteractionState(IGame game)
        {
            return new SelectInteractionState(_interactionContext, this, game);
        }

        public IInteractionState CreateAttackInteractionState(IGame game, IRegion selectedRegion)
        {
            return new AttackInteractionState(_interactionContext, this, game, selectedRegion);
        }

        public IInteractionState CreateSendArmiesToOccupyInteractionState(IGame game, IRegion selectedRegion, IRegion conqueredRegion)
        {
            return new SendArmiesToOccupyInteractionState(_interactionContext, this, game, selectedRegion, conqueredRegion);
        }

        public IInteractionState CreateFortifySelectInteractionState(IGame game)
        {
            return new FortifySelectInteractionState(_interactionContext, this, game);
        }

        public IInteractionState CreateFortifyMoveInteractionState(IGame game, IRegion selectedRegion)
        {
            return new FortifyMoveInteractionState(_interactionContext, this, game, selectedRegion);
        }

        public IInteractionState CreateEndTurnInteractionState()
        {
            return new EndTurnInteractionState();
        }
    }
}