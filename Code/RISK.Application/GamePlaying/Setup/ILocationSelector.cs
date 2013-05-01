using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface ILocationSelector
    {
        ILocation Select(SelectLocationParameter selectLocationParameter);
    }
}