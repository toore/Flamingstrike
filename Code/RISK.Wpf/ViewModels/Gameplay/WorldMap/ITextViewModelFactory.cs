using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.WorldMap
{
    public interface ITextViewModelFactory
    {
        TextViewModel Create(ITerritory territory);
    }
}