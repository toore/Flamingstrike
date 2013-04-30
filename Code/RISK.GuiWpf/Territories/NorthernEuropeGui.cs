using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class NorthernEuropeGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.NORTHERN_EUROPE; }
        }

        public Point NamePosition
        {
            get { return new Point(630, 120); }
        }

        public string Path
        {
            get { return "m 635.89103 163.92878 23.73858 -20.70813 48.9924 0 7.07107 43.94164 -62.62946 -3.53553 z"; }
        }
    }
}