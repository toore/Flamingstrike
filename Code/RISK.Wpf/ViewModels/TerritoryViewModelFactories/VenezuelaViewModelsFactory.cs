using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class VenezuelaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public VenezuelaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.VENEZUELA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(250.77037, 436.66996); }
        }

        protected override string Path
        {
            get { return "m 250.77037 436.66996 6.56599 -32.82995 30.55712 -29.04189 90.66119 46.21448 -87.12566 37.62818 z"; }
        }
    }
}