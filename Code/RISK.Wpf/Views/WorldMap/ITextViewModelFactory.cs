using GuiWpf.ViewModels;
using GuiWpf.ViewModels.WorldMapViewModels;
using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMap
{
    public interface ITextViewModelFactory
    {
        TextViewModel Create(ITerritory territory);
    }
}