using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnFactory : ITurnFactory
    {
        private readonly IBattleCalculator _battleCalculator;

        public TurnFactory(IBattleCalculator battleCalculator)
        {
            _battleCalculator = battleCalculator;
        }

        public ITurn Create(IPlayer player, IWorldMap worldMap)
        {
            return new Turn(player, worldMap, _battleCalculator);
        }
    }
}