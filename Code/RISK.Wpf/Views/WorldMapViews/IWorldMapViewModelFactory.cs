using System;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.Views.WorldMapViews
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IWorldMap worldMap, Action<ILocation> selectLocation);
    }
}