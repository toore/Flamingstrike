using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITextViewModelFactory
    {
        TextViewModel Create(ITerritory territory);
    }
}