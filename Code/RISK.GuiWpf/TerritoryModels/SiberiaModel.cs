using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class SiberiaModel : TerritoryModelBase
    {
        public SiberiaModel(ITerritoryGeography territoryGeography) : base(territoryGeography) {}

        public override string Name
        {
            get { return Resources.SIBERIA; }
        }

        public override Point NamePosition
        {
            get { return new Point(930, 40); }
        }

        public override string Path
        {
            get { return "m 887.92409 59.377989 60.60915 -25.253814 34.34519 18.687823 36.87057 71.720832 -40.40611 51.0127 -8.58629 2.52538 -3.53554 -48.48732 z"; }
        }
    }
}