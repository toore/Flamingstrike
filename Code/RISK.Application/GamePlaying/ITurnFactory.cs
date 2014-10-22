using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurnFactory
    {
        ITurn CreateSelectTurn(IPlayer player, IWorldMap worldMap);
    }
}