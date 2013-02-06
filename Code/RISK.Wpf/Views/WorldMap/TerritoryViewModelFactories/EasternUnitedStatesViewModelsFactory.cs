using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class EasternUnitedStatesViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public EasternUnitedStatesViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.EASTERN_UNITED_STATES; }
        }

        protected override Point NamePosition
        {
            get { return new Point(220.71833, 181.10137); }
        }

        protected override string Path
        {
            get { return "m 220.71833 181.10137 51.26524 36.11295 67.68022 -32.32488 4.79823 12.62691 -87.63074 96.72211 -75.76144 6.31345 -32.57742 -30.55712 63.38708 -28.03173 z"; }
        }
    }
}