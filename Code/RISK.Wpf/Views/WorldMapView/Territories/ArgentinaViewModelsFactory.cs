using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class ArgentinaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public ArgentinaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.ARGENTINA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(270, 528); }
        }

        protected override string Path
        {
            get { return "m 270 528 86.42857 41.78571 32.14286 56.07143 -38.92857 111.78572 -37.5 -37.14286 z"; }
        }
    }
}