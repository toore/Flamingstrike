using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class PeruViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public PeruViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.PERU; }
        }

        protected override Point NamePosition
        {
            get { return new Point(251.07143, 437.28571); }
        }

        protected override string Path
        {
            get { return "M 251.07143 437.28571 242.14286 478.35714 270 527.28571 357.14286 570.14286 291.42857 458.71429 z"; }
        }
    }
}