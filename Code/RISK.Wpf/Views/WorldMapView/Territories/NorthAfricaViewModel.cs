using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class NorthAfricaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.NORTH_AFRICA; }
        }
        public override Point NamePosition
        {
            get { return new Point(666.70068, 240.70037); }
        }
        public override string Path
        {
            get { return "m 666.70068 240.70037 -3.53553 66.16499 64.14468 29.29443 -5.05076 48.9924 -58.08377 48.48732 -123.23861 -53.03301 34.34518 -123.74369 z"; }
        }
    }
}