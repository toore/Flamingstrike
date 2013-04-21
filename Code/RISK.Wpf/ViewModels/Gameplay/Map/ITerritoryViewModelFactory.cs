using System;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModel Create(ITerritory territory, Action<ILocation> clickCommand);
    }
}