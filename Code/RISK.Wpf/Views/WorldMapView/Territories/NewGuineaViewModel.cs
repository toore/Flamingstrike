using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class NewGuineaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.NEW_GUINEA; }
        }
        public override Point NamePosition
        {
            get { return new Point(1235.4166, 444.24611); }
        }
        public override string Path
        {
            get { return "m 1235.4166 444.24611 -14.6472 25.75889 97.4797 47.47717 11.6167 -47.47717 z"; }
        }
    }
}