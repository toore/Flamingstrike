using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryDataViewModelFactory
    {
        ITerritoryDataViewModel Create(ITerritory territory);
    }
}