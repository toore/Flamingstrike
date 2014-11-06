using System.Windows;
using GuiWpf.Properties;

namespace GuiWpf.TerritoryModels
{
    public class NorthAfricaModel : ITerritoryModel
    {
        public string Name
        {
            get { return Resources.NORTH_AFRICA; }
        }

        public Point NamePosition
        {
            get { return new Point(570, 290); }
        }

        public string Path
        {
            get { return "m 666.70068 240.70037 -3.53553 66.16499 64.14468 29.29443 -5.05076 48.9924 -58.08377 48.48732 -123.23861 -53.03301 34.34518 -123.74369 z"; }
        }
    }
}