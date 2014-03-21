using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class KamchatkaGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.KAMCHATKA; }
        }

        public Point NamePosition
        {
            get { return new Point(1190, 60); }
        }

        public string Path
        {
            get { return "m 1174.8074 71.49982 108.0863 13.131983 -33.335 77.276667 -47.9822 -50.50762 -50.0026 31.31472 60.1041 43.94164 -36.3655 21.71828 -2.5254 -27.7792 -18.6878 -2.0203 -28.2843 -46.97209 z"; }
        }
    }
}