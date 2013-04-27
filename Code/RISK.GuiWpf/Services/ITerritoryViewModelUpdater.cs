using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;

namespace GuiWpf.Services
{
    public interface ITerritoryViewModelUpdater
    {
        void UpdateColor(ITerritoryLayoutViewModel territoryLayoutViewModel, ITerritory territory);
    }
}