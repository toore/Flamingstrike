using System.Windows;
using GuiWpf.Properties;
using RISK.Application.Entities;

namespace GuiWpf.TerritoryModels
{
    public class BrazilModel : TerritoryModelBase
    {
        public BrazilModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.BRAZIL; }
        }

        public override Point NamePosition
        {
            get { return new Point(360, 450); }
        }

        public override string Path
        {
            get { return "m 291.07143 458 87.5 -37.14286 76.78571 54.28572 L 388.92857 625.5 z"; }
        }
    }
}