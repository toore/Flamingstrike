using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class BrazilGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.BRAZIL; }
        }

        public Point NamePosition
        {
            get { return new Point(360, 450); }
        }

        public string Path
        {
            get { return "m 291.07143 458 87.5 -37.14286 76.78571 54.28572 L 388.92857 625.5 z"; }
        }
    }
}