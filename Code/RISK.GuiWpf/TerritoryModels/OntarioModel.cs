using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class OntarioModel : RegionModelBase
    {
        public OntarioModel(IRegion region) : base(region) {}

        public override string Name
        {
            get { return Resources.ONTARIO; }
        }

        public override Point NamePosition
        {
            get { return new Point(240, 130); }
        }

        public override string Path
        {
            get { return "m 244.70945 118.72445 30.55712 0.50508 30.30458 43.43656 5.55583 34.59772 -39.39595 19.1929 -58.3363 -41.16372 z"; }
        }
    }
}