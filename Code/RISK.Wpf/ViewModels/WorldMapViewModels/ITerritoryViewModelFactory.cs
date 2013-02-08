using System;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.WorldMapViewModels
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModel Create(ITerritory territory, Action<ILocation> clickCommand);
    }
}