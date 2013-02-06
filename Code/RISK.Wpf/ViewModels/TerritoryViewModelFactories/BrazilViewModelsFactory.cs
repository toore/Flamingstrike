using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class BrazilViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public BrazilViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.BRAZIL; }
        }

        protected override Point NamePosition
        {
            get { return new Point(291.07143, 458); }
        }

        protected override string Path
        {
            get { return "m 291.07143 458 87.5 -37.14286 76.78571 54.28572 L 388.92857 625.5 z"; }
        }
    }
}