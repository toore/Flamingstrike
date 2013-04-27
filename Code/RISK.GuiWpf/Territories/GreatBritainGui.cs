using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.Territories
{
    public class GreatBritainGui : ITerritoryGui
    {
        public string Name
        {
            get { return Resources.GREAT_BRITAIN; }
        }

        public Point NamePosition
        {
            get { return new Point(584.37325, 142.2105); }
        }

        public string Path
        {
            get
            {
                return "m 584.37325 142.2105 -3.53554 21.2132 13.63706 0.50508 9.09138 -17.67767 z " +
                       "m 25.25381 -16.66752 -3.03046 18.68782 -3.03045 25.75889 32.82995 -13.63706 -19.69797 -29.29442 z";
            }
        }
    }
}