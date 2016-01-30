using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class NewGuineaModel : RegionModelBase
    {
        public NewGuineaModel(IRegion region) : base(region) {}

        public override string Name
        {
            get { return Resources.NEW_GUINEA; }
        }

        public override Point NamePosition
        {
            get { return new Point(1240, 430); }
        }

        public override string Path
        {
            get { return "m 1235.4166 444.24611 -14.6472 25.75889 97.4797 47.47717 11.6167 -47.47717 z"; }
        }
    }
}