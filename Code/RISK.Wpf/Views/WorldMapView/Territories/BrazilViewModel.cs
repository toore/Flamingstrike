using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class BrazilViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.BRAZIL; }
        }
        public override Point NamePosition
        {
            get { return new Point(291.07143, 458); }
        }
        public override string Path
        {
            get { return "m 291.07143 458 87.5 -37.14286 76.78571 54.28572 L 388.92857 625.5 z"; }
        }
    }
}