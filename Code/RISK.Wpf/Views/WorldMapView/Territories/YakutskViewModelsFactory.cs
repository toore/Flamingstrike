using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class YakutskViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public YakutskViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.YAKUTSK; }
        }

        protected override Point NamePosition
        {
            get { return new Point(981.86828, 51.801845); }
        }

        protected override string Path
        {
            get { return "M 981.86828 51.801845 1175.3125 71.49982 1125.815 131.6039 1011.6678 107.36023 z"; }
        }
    }
}