using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class WesternUnitedStatesViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public WesternUnitedStatesViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.WESTERN_UNITED_STATES; }
        }

        protected override Point NamePosition
        {
            get { return new Point(126.26907, 176.05061); }
        }

        protected override string Path
        {
            get { return "m 126.26907 176.05061 86.87312 0.25254 7.57614 4.54568 -8.83883 61.87185 -63.38708 27.77919 -59.851534 -22.47589 z"; }
        }
    }
}