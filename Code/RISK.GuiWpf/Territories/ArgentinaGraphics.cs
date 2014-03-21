using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class ArgentinaGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.ARGENTINA; }
        }

        public Point NamePosition
        {
            get { return new Point(300, 590); }
        }

        public string Path
        {
            get { return "m 270 528 86.42857 41.78571 32.14286 56.07143 -38.92857 111.78572 -37.5 -37.14286 z"; }
        }
    }
}