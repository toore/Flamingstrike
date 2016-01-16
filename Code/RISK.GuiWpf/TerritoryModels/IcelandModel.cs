using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class IcelandModel : TerritoryModelBase
    {
        public IcelandModel(ITerritoryGeography territoryGeography) : base(territoryGeography) {}

        public override string Name
        {
            get { return Resources.ICELAND; }
        }

        public override Point NamePosition
        {
            get { return new Point(560, 40); }
        }

        public override string Path
        {
            get { return "M 546.9976 90.692718 552.55344 107.36023 585.3834 94.733328 575.28187 82.106421 z"; }
        }
    }
}