using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.TerritoryModels
{
    public class VenezuelaModel : ITerritoryModel
    {
        public string Name
        {
            get { return Resources.VENEZUELA; }
        }

        public Point NamePosition
        {
            get { return new Point(270, 370); }
        }

        public string Path
        {
            get { return "m 250.77037 436.66996 6.56599 -32.82995 30.55712 -29.04189 90.66119 46.21448 -87.12566 37.62818 z"; }
        }
    }
}