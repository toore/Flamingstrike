using System.Windows.Media;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryLayoutViewModel : IWorldMapItemViewModel
    {
        Color NormalStrokeColor { get; set; }
        Color NormalFillColor { get; set; }
        Color MouseOverStrokeColor { get; set; }
        Color MouseOverFillColor { get; set; }

        bool IsSelected { get; }
        ITerritory Territory { get; }

        void OnClick();
    }
}