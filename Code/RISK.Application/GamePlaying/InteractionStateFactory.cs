using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class InteractionStateFactory : IInteractionStateFactory
    {
        private readonly StateController _stateController;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;

        public InteractionStateFactory(StateController stateController, IBattleCalculator battleCalculator, ICardFactory cardFactory)
        {
            _stateController = stateController;
            _battleCalculator = battleCalculator;
            _cardFactory = cardFactory;
        }

        public IInteractionState CreateSelectState(IPlayer player, IWorldMap worldMap)
        {
            return new SelectState(_stateController, this, _cardFactory, player, worldMap);
        }

        public IInteractionState CreateAttackState(IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory)
        {
            return new AttackState(_stateController, this, _battleCalculator, _cardFactory, player, worldMap, selectedTerritory);
        }

        public IInteractionState CreateFortifiedState(IPlayer player, IWorldMap worldMap)
        {
            return new FortifiedState(_stateController, player);
        }
    }
}