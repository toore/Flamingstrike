using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class GreenlandViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public GreenlandViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.GREENLAND; }
        }

        protected override Point NamePosition
        {
            get { return new Point(418.20315, 33.6191); }
        }

        protected override string Path
        {
            get { return "m 418.20315 33.6191 42.42641 82.83251 104.04571 -65.659917 18.18275 -38.385797 z"; }
        }
    }
}