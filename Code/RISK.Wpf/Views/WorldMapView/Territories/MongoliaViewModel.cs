using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class MongoliaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.MONGOLIA; }
        }
        public override Point NamePosition
        {
            get { return new Point(1110.6627, 214.4364); }
        }
        public override string Path
        {
            get { return "m 1110.6627 214.4364 67.6802 41.92134 -5.5558 -75.76145 -184.35283 -16.66751 -9.09138 11.61675 69.70051 51.51778 z"; }
        }
    }
}