using System.Windows;
using GuiWpf.Properties;
using RISK.Application.Entities;

namespace GuiWpf.TerritoryModels
{
    public class IndiaModel : TerritoryModelBase
    {
        public IndiaModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.INDIA; }
        }

        public override Point NamePosition
        {
            get { return new Point(940, 270); }
        }

        public override string Path
        {
            get { return "m 889.94439 249.28667 36.36549 -28.78935 126.26912 73.74114 -81.31732 110.6117 -75.76145 -98.99495 z"; }
        }
    }
}