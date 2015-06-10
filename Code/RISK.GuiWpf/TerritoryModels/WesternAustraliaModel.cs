using System.Windows;
using GuiWpf.Properties;
using RISK.Application;

namespace GuiWpf.TerritoryModels
{
    public class WesternAustraliaModel : TerritoryModelBase
    {
        public WesternAustraliaModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.WESTERN_AUSTRALIA; }
        }

        public override Point NamePosition
        {
            get { return new Point(1080, 560); }
        }

        public override string Path
        {
            get { return "m 1207.6374 526.06847 -74.7513 40.91117 -10.1015 68.69038 106.571 20.70812 z"; }
        }
    }
}