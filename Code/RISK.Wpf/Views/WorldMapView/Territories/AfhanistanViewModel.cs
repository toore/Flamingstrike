using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class AfhanistanViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.AFGHANISTAN; }
        }
        public override Point NamePosition
        {
            get { return new Point(802.06112, 212.92118); }
        }
        public override string Path
        {
            get { return "m 802.06112 212.92118 64.14469 -48.48733 96.97464 26.26397 -72.73098 58.08377 z"; }
        }
    }
}