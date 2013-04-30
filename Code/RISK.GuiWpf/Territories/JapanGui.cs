using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class JapanGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.JAPAN; }
        }

        public Point NamePosition
        {
            get { return new Point(1220, 220); }
        }

        public string Path
        {
            get { return "m 1208.6475 196.25366 11.1117 41.92133 -28.2843 20.70813 7.0711 16.66751 36.8706 -27.27411 -6.061 -45.96195 z"; }
        }
    }
}