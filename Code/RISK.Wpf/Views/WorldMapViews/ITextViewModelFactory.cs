using GuiWpf.ViewModels.WorldMapViewModels;
using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMapViews
{
    public interface ITextViewModelFactory
    {
        TextViewModel Create(ITerritory territory);
    }
}