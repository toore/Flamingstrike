using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class ArgentinaModel : RegionModelBase
    {
        public ArgentinaModel(IRegion region) : base(region) {}

        public override string Name
        {
            get { return Resources.ARGENTINA; }
        }

        public override Point NamePosition
        {
            get { return new Point(300, 590); }
        }

        public override string Path
        {
            get { return "m 270 528 86.42857 41.78571 32.14286 56.07143 -38.92857 111.78572 -37.5 -37.14286 z"; }
        }
    }
}