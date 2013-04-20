using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;

namespace GuiWpf.Services
{
    public interface ITerritoryViewModelUpdater
    {
        void UpdateColor(ITerritoryViewModel territoryViewModel, ITerritory territory);
    }
}