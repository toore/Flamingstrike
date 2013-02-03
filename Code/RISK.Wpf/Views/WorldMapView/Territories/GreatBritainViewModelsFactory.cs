using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class GreatBritainViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public GreatBritainViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.GREAT_BRITAIN; }
        }

        protected override Point NamePosition
        {
            get { return new Point(584.37325, 142.2105); }
        }

        protected override string Path
        {
            get
            {
                return "m 584.37325 142.2105 -3.53554 21.2132 13.63706 0.50508 9.09138 -17.67767 z " +
                       "m 25.25381 -16.66752 -3.03046 18.68782 -3.03045 25.75889 32.82995 -13.63706 -19.69797 -29.29442 z";
            }
        }
    }
}