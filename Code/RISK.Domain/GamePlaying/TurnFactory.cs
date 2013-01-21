using System;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnFactory : ITurnFactory
    {
        private readonly IWorldMap _worldMap;
        private readonly IBattleEvaluater _battleEvaluater;

        public TurnFactory(IWorldMap worldMap, IBattleEvaluater battleEvaluater)
        {
            _worldMap = worldMap;
            _battleEvaluater = battleEvaluater;
        }

        public ITurn Create(IPlayer player)
        {
            return new Turn(player, _worldMap, _battleEvaluater);
        }
    }
}