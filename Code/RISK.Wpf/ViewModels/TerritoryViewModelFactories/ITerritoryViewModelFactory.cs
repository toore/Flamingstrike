using System;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModel Create(ITerritory territory, Action clickCommand);
    }
}