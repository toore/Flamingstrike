using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class SouthAfricaModel : TerritoryModelBase
    {
        public SouthAfricaModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.SOUTH_AFRICA; }
        }

        public override Point NamePosition
        {
            get { return new Point(690, 510); }
        }

        public override string Path
        {
            get { return "m 673.77175 479.09637 26.76904 154.55334 32.82996 2.02031 47.47717 -65.15484 23.73858 -66.67007 -48.48732 -13.13198 -33.33503 15.15228 z"; }
        }
    }
}