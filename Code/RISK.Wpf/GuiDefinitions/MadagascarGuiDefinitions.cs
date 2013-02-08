using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.GuiDefinitions
{
    public class MadagascarGuiDefinitions : ITerritoryGuiDefinitions
    {
        public string Name
        {
            get { return Resources.MADAGASCAR; }
        }

        public Point NamePosition
        {
            get { return new Point(843.47738, 514.45171); }
        }

        public string Path
        {
            get { return "m 843.47738 514.45171 -23.23351 25.75889 -6.06092 42.93148 24.74874 3.03046 z"; }
        }
    }
}