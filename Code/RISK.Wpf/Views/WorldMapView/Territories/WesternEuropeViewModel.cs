using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class WesternEuropeViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.WESTERN_EUROPE; }
        }

        public override Point NamePosition
        {
            get { return new Point(636.3961, 164.93893); }
        }
        
        public override string Path
        {
            get { return "m 636.3961 164.93893 -29.7995 14.14214 7.07107 26.76904 -24.74874 0 -8.08122 36.36549 33.84011 -2.02031 37.37565 -35.35533 0.50507 -22.22336 z"; }
        }
    }
}