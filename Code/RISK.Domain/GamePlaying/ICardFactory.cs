using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ICardFactory
    {
        Card Create();
    }
}