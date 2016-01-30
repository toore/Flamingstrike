using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class MadagascarModel : RegionModelBase
    {
        public MadagascarModel(IRegion region) : base(region) {}

        public override string Name
        {
            get { return Resources.MADAGASCAR; }
        }

        public override Point NamePosition
        {
            get { return new Point(830, 520); }
        }

        public override string Path
        {
            get { return "m 843.47738 514.45171 -23.23351 25.75889 -6.06092 42.93148 24.74874 3.03046 z"; }
        }
    }
}