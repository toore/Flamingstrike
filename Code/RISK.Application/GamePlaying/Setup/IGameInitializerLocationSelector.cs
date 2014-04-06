using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface IGameInitializerLocationSelector
    {
        ILocation SelectLocation(ILocationSelectorParameter locationSelectorParameter);
    }
}