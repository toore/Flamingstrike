using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class WesternUnitedStatesLayoutInformation : ITerritoryLayoutInformation
    {
        public string Name
        {
            get { return Resources.WESTERN_UNITED_STATES; }
        }

        public Point NamePosition
        {
            get { return new Point(126.26907, 176.05061); }
        }

        public string Path
        {
            get { return "m 126.26907 176.05061 86.87312 0.25254 7.57614 4.54568 -8.83883 61.87185 -63.38708 27.77919 -59.851534 -22.47589 z"; }
        }
    }
}