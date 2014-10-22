using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnStateFactory : ITurnStateFactory
    {
        private readonly StateController _stateController;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;

        public TurnStateFactory(StateController stateController, IBattleCalculator battleCalculator, ICardFactory cardFactory)
        {
            _stateController = stateController;
            _battleCalculator = battleCalculator;
            _cardFactory = cardFactory;
        }

        public ITurnState CreateSelectState(IPlayer player, IWorldMap worldMap)
        {
            return new TurnSelectState(_stateController, this, _cardFactory, player, worldMap);
        }

        public ITurnState CreateAttackState(IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory)
        {
            return new TurnAttackState(_stateController, this, _battleCalculator, _cardFactory, player, worldMap, selectedTerritory);
        }
    }
}