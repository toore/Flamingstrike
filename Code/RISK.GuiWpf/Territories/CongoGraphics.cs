using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class CongoGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.CONGO; }
        }

        public Point NamePosition
        {
            get { return new Point(680, 420); }
        }

        public string Path
        {
            get { return "m 664.1753 433.6395 58.58885 -48.48731 33.84011 104.55078 -34.34519 16.16244 -47.98225 -26.76903 z"; }
        }
    }
}