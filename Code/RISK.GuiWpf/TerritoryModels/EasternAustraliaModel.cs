using System.Windows;
using GuiWpf.Properties;
using RISK.Application.Entities;

namespace GuiWpf.TerritoryModels
{
    public class EasternAustraliaModel : TerritoryModelBase
    {
        public EasternAustraliaModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.EASTERN_AUSTRALIA; }
        }

        public override Point NamePosition
        {
            get { return new Point(1240, 530); }
        }

        public override string Path
        {
            get { return "m 1206.6272 526.06847 72.2259 -8.08123 29.7995 78.28683 -79.802 61.11423 z"; }
        }
    }
}