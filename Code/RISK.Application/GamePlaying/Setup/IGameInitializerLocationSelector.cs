using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface IGameInitializerLocationSelector
    {
        ITerritory SelectLocation(ILocationSelectorParameter locationSelectorParameter);
    }
}