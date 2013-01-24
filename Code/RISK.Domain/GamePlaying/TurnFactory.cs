using System;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnFactory : ITurnFactory
    {
        private readonly IWorldMap _worldMap;
        private readonly IBattleCalculator _battleCalculator;

        public TurnFactory(IWorldMap worldMap, IBattleCalculator battleCalculator)
        {
            _worldMap = worldMap;
            _battleCalculator = battleCalculator;
        }

        public ITurn Create(IPlayer player)
        {
            return new Turn(player, _worldMap, _battleCalculator);
        }
    }
}