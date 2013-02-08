using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.GuiDefinitions
{
    public class IndonesiaGuiDefinitions : ITerritoryGuiDefinitions
    {
        public string Name
        {
            get { return Resources.INDONESIA; }
        }

        public Point NamePosition
        {
            get { return new Point(1156.6247, 406.87046); }
        }

        public string Path
        {
            get
            {
                return "m 1156.6247 406.87046 -40.4061 43.43656 l 34.3451 18.68783 22.2234 -50.00255 z " +
                       "M 0 0 m 1163.6957 344.74608 26.7691 71.72083 20.203 -8.58629 -36.3655 -65.15484 z " +
                       "M 0 0 m 1052.579 421.0126 40.9111 56.06347 19.698 -10.10153 -45.9619 -51.51778 z " +
                       "M 0 0 m 1111.1678 477.58114 -10.6066 12.62691 96.9746 16.16244 7.0711 -18.18275 z";
            }
        }
    }
}