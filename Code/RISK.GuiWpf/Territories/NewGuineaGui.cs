using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class NewGuineaGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.NEW_GUINEA; }
        }

        public Point NamePosition
        {
            get { return new Point(1240, 430); }
        }

        public string Path
        {
            get { return "m 1235.4166 444.24611 -14.6472 25.75889 97.4797 47.47717 11.6167 -47.47717 z"; }
        }
    }
}