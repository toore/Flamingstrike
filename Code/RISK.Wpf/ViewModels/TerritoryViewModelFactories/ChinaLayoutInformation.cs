using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class ChinaLayoutInformation : ITerritoryLayoutInformation
    {
        public string Name
        {
            get { return Resources.CHINA; }
        }

        public Point NamePosition
        {
            get { return new Point(925.80481, 219.99224); }
        }

        public string Path
        {
            get { return "m 925.80481 219.99224 180.81729 106.57109 45.4569 -11.11167 8.0812 -40.91118 -47.9822 -60.10408 -63.4951 12.04032 -69.84508 -50.42611 -7.07107 2.52538 0 7.57614 -9.09137 3.53554 z"; }
        }
    }
}