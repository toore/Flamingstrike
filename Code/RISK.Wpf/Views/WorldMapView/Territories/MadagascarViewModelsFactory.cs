using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class MadagascarViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public MadagascarViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.MADAGASCAR; }
        }

        protected override Point NamePosition
        {
            get { return new Point(843.47738, 514.45171); }
        }

        protected override string Path
        {
            get { return "m 843.47738 514.45171 -23.23351 25.75889 -6.06092 42.93148 24.74874 3.03046 z"; }
        }
    }
}