using System.Windows.Media;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryViewModel : IWorldMapViewModel
    {
        ILocation Location { get; }
        
        Color NormalStrokeColor { get; set; }
        Color NormalFillColor { get; set; }
        Color MouseOverStrokeColor { get; set; }
        Color MouseOverFillColor { get; set; }

        bool IsEnabled { get; set; }
        
        void OnClick();
    }
}