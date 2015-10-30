using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class JapanModel : TerritoryModelBase
    {
        public JapanModel(ITerritoryId territoryId) : base(territoryId) {}

        public override string Name
        {
            get { return Resources.JAPAN; }
        }

        public override Point NamePosition
        {
            get { return new Point(1220, 220); }
        }

        public override string Path
        {
            get { return "m 1208.6475 196.25366 11.1117 41.92133 -28.2843 20.70813 7.0711 16.66751 36.8706 -27.27411 -6.061 -45.96195 z"; }
        }
    }
}