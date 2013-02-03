using System;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.Views.WorldMapView
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> selectTerritory);
    }
}