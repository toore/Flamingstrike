using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class VenezuelaModel : TerritoryModelBase
    {
        public VenezuelaModel(ITerritoryId territoryId) : base(territoryId) {}

        public override string Name
        {
            get { return Resources.VENEZUELA; }
        }

        public override Point NamePosition
        {
            get { return new Point(270, 370); }
        }

        public override string Path
        {
            get { return "m 250.77037 436.66996 6.56599 -32.82995 30.55712 -29.04189 90.66119 46.21448 -87.12566 37.62818 z"; }
        }
    }
}