using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameboard.WorldMap
{
    public interface ITextViewModelFactory
    {
        TextViewModel Create(ITerritory territory);
    }
}