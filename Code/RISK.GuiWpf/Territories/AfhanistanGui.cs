using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class AfhanistanGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.AFGHANISTAN; }
        }

        public Point NamePosition
        {
            get { return new Point(830, 170); }
        }

        public string Path
        {
            get { return "m 802.06112 212.92118 64.14469 -48.48733 96.97464 26.26397 -72.73098 58.08377 z"; }
        }
    }
}