using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryTextViewModelFactory
    {
        ITerritoryTextViewModel Create(ITerritory territory);
    }
}