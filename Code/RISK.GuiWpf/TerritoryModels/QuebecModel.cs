using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class QuebecModel : TerritoryModelBase
    {
        public QuebecModel(ITerritory territory) : base(territory) {}

        public override string Name
        {
            get { return Resources.QUEBEC; }
        }

        public override Point NamePosition
        {
            get { return new Point(320, 100); }
        }

        public override string Path
        {
            get { return "m 306.5813 163.17116 41.16371 -57.83123 56.56855 50.00255 -92.6815 42.17387 z"; }
        }
    }
}