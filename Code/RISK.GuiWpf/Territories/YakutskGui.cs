using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class YakutskGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.YAKUTSK; }
        }

        public Point NamePosition
        {
            get { return new Point(1060, 40); }
        }

        public string Path
        {
            get { return "M 981.86828 51.801845 1175.3125 71.49982 1125.815 131.6039 1011.6678 107.36023 z"; }
        }
    }
}