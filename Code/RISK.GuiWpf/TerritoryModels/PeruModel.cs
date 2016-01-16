using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class PeruModel : TerritoryModelBase
    {
        public PeruModel(ITerritoryGeography territoryGeography) : base(territoryGeography) {}

        public override string Name
        {
            get { return Resources.PERU; }
        }

        public override Point NamePosition
        {
            get { return new Point(260, 460); }
        }

        public override string Path
        {
            get { return "M 251.07143 437.28571 242.14286 478.35714 270 527.28571 357.14286 570.14286 291.42857 458.71429 z"; }
        }
    }
}