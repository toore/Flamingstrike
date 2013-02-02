using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class EastAfricaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.EAST_AFRICA; }
        }
        public override Point NamePosition
        {
            get { return new Point(727.30983, 337.16994); }
        }
        public override string Path
        {
            get { return "m 727.30983 337.16994 53.53809 -19.69798 40.91118 69.19545 32.32488 -6.56599 -51.01271 124.24877 -46.46701 -13.63706 -33.33504 -104.55079 z"; }
        }
    }
}