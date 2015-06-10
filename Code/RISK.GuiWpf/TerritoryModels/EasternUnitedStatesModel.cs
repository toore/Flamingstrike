using System.Windows;
using GuiWpf.Properties;
using RISK.Application;

namespace GuiWpf.TerritoryModels
{
    public class EasternUnitedStatesModel : TerritoryModelBase
    {
        public EasternUnitedStatesModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.EASTERN_UNITED_STATES; }
        }

        public override Point NamePosition
        {
            get { return new Point(220, 220); }
        }

        public override string Path
        {
            get { return "m 220.71833 181.10137 51.26524 36.11295 67.68022 -32.32488 4.79823 12.62691 -87.63074 96.72211 -75.76144 6.31345 -32.57742 -30.55712 63.38708 -28.03173 z"; }
        }
    }
}