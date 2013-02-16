using System;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.WorldMap
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModel Create(ITerritory territory, Action<ILocation> clickCommand);
    }
}