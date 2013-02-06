using System;
using System.Windows.Media;
using GuiWpf.ViewModels.TerritoryViewModelFactories;

namespace GuiWpf.ViewModels
{
    public class TerritoryViewModel : IWorldMapViewModel
    {
        public string Path { get; set; }

        public Color NormalStrokeColor { get; set; }
        public Color NormalFillColor { get; set; }
        public Color MouseOverStrokeColor { get; set; }
        public Color MouseOverFillColor { get; set; }
        public Action ClickCommand { get; set; }

        public bool IsEnabled { get; set; }

        public void OnClick()
        {
            ClickCommand();
        }
    }
}