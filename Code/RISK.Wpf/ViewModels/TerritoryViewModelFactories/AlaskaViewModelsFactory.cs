using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class AlaskaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public AlaskaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.ALASKA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(12.121866, 139.68511); }
        }

        protected override string Path
        {
            get { return "m 12.121866 139.68511 41.67913 -42.115279 12.86906 -25.564934 50.513114 -8.563276 40.40058 8.563276 -45.57044 44.381753 5.76544 11.7349 -44.064802 -14.83465 z"; }
        }
    }
}