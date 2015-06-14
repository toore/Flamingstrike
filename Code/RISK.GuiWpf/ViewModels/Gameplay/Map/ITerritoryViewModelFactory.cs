using System;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryLayoutViewModel Create(ITerritory territory, Action<ITerritory> clickCommand);
    }
}