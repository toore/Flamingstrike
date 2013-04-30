using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class WesternEuropeGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.WESTERN_EUROPE; }
        }

        public Point NamePosition
        {
            get { return new Point(500, 180); }
        }

        public string Path
        {
            get { return "m 636.3961 164.93893 -29.7995 14.14214 7.07107 26.76904 -24.74874 0 -8.08122 36.36549 33.84011 -2.02031 37.37565 -35.35533 0.50507 -22.22336 z"; }
        }
    }
}