using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class SiamGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.SIAM; }
        }

        public Point NamePosition
        {
            get { return new Point(1060, 320); }
        }

        public string Path
        {
            get { return "m 1052.0738 293.73338 55.0534 33.33503 l 13.132 56.06347 -17.6777 14.64721 -24.2437 -24.24366 23.2336 63.13453 -19.1929 -11.11167 -50.0026 -102.53049 z"; }
        }
    }
}