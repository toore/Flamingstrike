using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public class InteractionStateFactory : IInteractionStateFactory
    {
        private readonly IBattleCalculator _battleCalculator;

        public InteractionStateFactory(IBattleCalculator battleCalculator)
        {
            _battleCalculator = battleCalculator;
        }

        public IInteractionState CreateSelectState(StateController stateController, IPlayer player)
        {
            return new SelectState(stateController, this, player);
        }

        public IInteractionState CreateAttackState(StateController stateController, IPlayer player, ITerritory selectedTerritory)
        {
            return new AttackState(stateController, this, _battleCalculator, player, selectedTerritory);
        }

        public IInteractionState CreateFortifyState(StateController stateController, IPlayer player)
        {
            return new FortifySelectState(stateController, this, player);
        }

        public IInteractionState CreateFortifyState(StateController stateController, IPlayer player, ITerritory selectedTerritory)
        {
            return new FortifyMoveState(player, selectedTerritory);
        }
    }
}