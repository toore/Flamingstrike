using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnStateFactory : ITurnStateFactory
    {
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;

        public TurnStateFactory(IBattleCalculator battleCalculator, ICardFactory cardFactory)
        {
            _battleCalculator = battleCalculator;
            _cardFactory = cardFactory;
        }

        public ITurnState CreateSelectState(IPlayer player, IWorldMap worldMap)
        {
            return new TurnSelectState(player, worldMap, _battleCalculator, _cardFactory);
        }
    }
}