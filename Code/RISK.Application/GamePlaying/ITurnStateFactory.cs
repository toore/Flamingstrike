using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurnStateFactory
    {
        ITurnState CreateSelectState(IPlayer player, IWorldMap worldMap);
        ITurnState CreateAttackState(IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory);
    }
}