using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class GreenlandGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.GREENLAND; }
        }

        public Point NamePosition
        {
            get { return new Point(450, 20); }
        }

        public string Path
        {
            get { return "m 418.20315 33.6191 42.42641 82.83251 104.04571 -65.659917 18.18275 -38.385797 z"; }
        }
    }
}