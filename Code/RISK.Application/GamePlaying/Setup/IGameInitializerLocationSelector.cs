using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public interface IGameInitializerLocationSelector
    {
        ITerritory SelectLocation(ILocationSelectorParameter locationSelectorParameter);
    }
}