using System;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryLayoutViewModel Create(ITerritory territory, Action<ITerritory> clickCommand);
    }
}