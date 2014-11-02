using System;
using System.Collections.Generic;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IEnumerable<ITerritory> territories, Action<ITerritory> selectLocation);
    }
}