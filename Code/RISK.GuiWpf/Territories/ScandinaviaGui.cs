using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class ScandinaviaGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.SCANDINAVIA; }
        }

        public Point NamePosition
        {
            get { return new Point(630, 50); }
        }

        public string Path
        {
            get { return "m 645.99255 110.39069 58.58885 -45.456862 27.27412 2.020305 1.51523 49.497477 -26.26397 0 7.07107 -25.758892 -9.59645 -1.515229 -18.18275 52.022861 -17.17259 -1.51523 -3.53553 -15.15229 -14.64721 3.53553 z"; }
        }
    }
}