using System;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModel Create(ITerritory territory, Action<ITerritory> clickCommand);
    }
}