using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnFactory : ITurnFactory
    {
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;

        public TurnFactory(IBattleCalculator battleCalculator, ICardFactory cardFactory)
        {
            _battleCalculator = battleCalculator;
            _cardFactory = cardFactory;
        }

        public ITurn Create(IPlayer player, IWorldMap worldMap)
        {
            return new Turn(player, worldMap, _battleCalculator, _cardFactory);
        }
    }
}