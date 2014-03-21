using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class IcelandGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.ICELAND; }
        }

        public Point NamePosition
        {
            get { return new Point(560, 40); }
        }

        public string Path
        {
            get { return "M 546.9976 90.692718 552.55344 107.36023 585.3834 94.733328 575.28187 82.106421 z"; }
        }
    }
}