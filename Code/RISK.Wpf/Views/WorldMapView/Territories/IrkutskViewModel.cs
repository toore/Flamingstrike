using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class IrkutskViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.IRKUTSK; }
        }
        public override Point NamePosition
        {
            get { return new Point(1012.1729, 108.37039); }
        }
        public override string Path
        {
            get { return "m 1012.1729 108.37039 113.6421 23.73858 27.7792 45.96194 -165.66501 -14.64721 31.81981 -40.4061 z"; }
        }
    }
}