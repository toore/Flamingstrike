using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class NorthAfricaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public NorthAfricaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.NORTH_AFRICA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(666.70068, 240.70037); }
        }

        protected override string Path
        {
            get { return "m 666.70068 240.70037 -3.53553 66.16499 64.14468 29.29443 -5.05076 48.9924 -58.08377 48.48732 -123.23861 -53.03301 34.34518 -123.74369 z"; }
        }
    }
}