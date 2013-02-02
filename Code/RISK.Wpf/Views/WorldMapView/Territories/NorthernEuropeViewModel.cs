using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class NorthernEuropeViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.NORTHERN_EUROPE; }
        }

        public override Point NamePosition
        {
            get { return new Point(635.89103, 163.92878); }
        }
        
        public override string Path
        {
            get { return "m 635.89103 163.92878 23.73858 -20.70813 48.9924 0 7.07107 43.94164 -62.62946 -3.53553 z"; }
        }
    }
}