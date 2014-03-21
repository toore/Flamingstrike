using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class QuebecGraphics : ITerritoryGraphics
    {
        public string Name
        {
            get { return Resources.QUEBEC; }
        }

        public Point NamePosition
        {
            get { return new Point(320, 100); }
        }

        public string Path
        {
            get { return "m 306.5813 163.17116 41.16371 -57.83123 56.56855 50.00255 -92.6815 42.17387 z"; }
        }
    }
}