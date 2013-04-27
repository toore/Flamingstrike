using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class PeruGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.PERU; }
        }

        public Point NamePosition
        {
            get { return new Point(251.07143, 437.28571); }
        }

        public string Path
        {
            get { return "M 251.07143 437.28571 242.14286 478.35714 270 527.28571 357.14286 570.14286 291.42857 458.71429 z"; }
        }
    }
}