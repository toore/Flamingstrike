using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application.Entities;

namespace GuiWpf.Services
{
    public interface ITerritoryViewModelUpdater
    {
        void UpdateColors(ITerritoryLayoutViewModel territoryLayoutViewModel, ITerritory territory);
    }
}