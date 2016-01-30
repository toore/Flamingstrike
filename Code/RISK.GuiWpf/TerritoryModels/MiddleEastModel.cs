using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class MiddleEastModel : RegionModelBase
    {
        public MiddleEastModel(IRegion region) : base(region) {}

        public override string Name
        {
            get { return Resources.MIDDLE_EAST; }
        }

        public override Point NamePosition
        {
            get { return new Point(790, 240); }
        }

        public override string Path
        {
            get { return "m 720.74384 223.0227 19.69798 -18.18274 59.599 8.08122 89.90357 35.35534 6.06092 57.57869 -57.5787 -26.76904 -7.07106 8.58629 58.58884 35.86042 -61.6193 49.49747 -52.52794 -86.36804 -5.05076 -14.64721 5.55584 -27.7792 z"; }
        }
    }
}