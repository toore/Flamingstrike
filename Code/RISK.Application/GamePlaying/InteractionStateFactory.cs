using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class InteractionStateFactory : IInteractionStateFactory
    {
        private readonly IBattleCalculator _battleCalculator;

        public InteractionStateFactory(IBattleCalculator battleCalculator)
        {
            _battleCalculator = battleCalculator;
        }

        public IInteractionState CreateSelectState(StateController stateController, IPlayer player, IWorldMap worldMap)
        {
            return new SelectState(stateController, this, player, worldMap);
        }

        public IInteractionState CreateAttackState(StateController stateController, IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory)
        {
            return new AttackState(stateController, this, _battleCalculator, player, worldMap, selectedTerritory);
        }
    }
}