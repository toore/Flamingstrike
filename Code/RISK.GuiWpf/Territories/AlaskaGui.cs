using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class AlaskaGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.ALASKA; }
        }

        public Point NamePosition
        {
            get { return new Point(12.121866, 139.68511); }
        }

        public string Path
        {
            get { return "m 12.121866 139.68511 41.67913 -42.115279 12.86906 -25.564934 50.513114 -8.563276 40.40058 8.563276 -45.57044 44.381753 5.76544 11.7349 -44.064802 -14.83465 z"; }
        }
    }
}