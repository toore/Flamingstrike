using System;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameboard.WorldMap
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModel Create(ITerritory territory, Action<ILocation> clickCommand);
    }
}