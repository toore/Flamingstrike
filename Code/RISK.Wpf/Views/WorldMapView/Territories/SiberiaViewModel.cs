using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class SiberiaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.SIBERIA; }
        }
        public override Point NamePosition
        {
            get { return new Point(887.92409, 59.377989); }
        }
        public override string Path
        {
            get { return "m 887.92409 59.377989 60.60915 -25.253814 34.34519 18.687823 36.87057 71.720832 -40.40611 51.0127 -8.58629 2.52538 -3.53554 -48.48732 z"; }
        }
    }
}