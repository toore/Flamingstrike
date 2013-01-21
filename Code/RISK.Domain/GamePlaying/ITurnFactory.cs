using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurnFactory
    {
        ITurn Create(IPlayer player);
    }
}