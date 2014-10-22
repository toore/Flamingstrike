using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurnStateFactory
    {
        ITurnState CreateSelectState(IPlayer player, IWorldMap worldMap);
    }
}