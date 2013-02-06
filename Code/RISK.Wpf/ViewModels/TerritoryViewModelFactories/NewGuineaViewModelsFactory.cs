using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class NewGuineaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public NewGuineaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.NEW_GUINEA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(1235.4166, 444.24611); }
        }

        protected override string Path
        {
            get { return "m 1235.4166 444.24611 -14.6472 25.75889 97.4797 47.47717 11.6167 -47.47717 z"; }
        }
    }
}