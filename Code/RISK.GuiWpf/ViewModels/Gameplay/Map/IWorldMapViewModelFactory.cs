using System;
using System.Collections.Generic;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IEnumerable<ITerritory> territories, Action<ITerritory> selectLocation);
    }
}