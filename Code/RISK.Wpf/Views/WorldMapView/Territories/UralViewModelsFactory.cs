using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class UralViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public UralViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.URAL; }
        }

        protected override Point NamePosition
        {
            get { return new Point(843.47738, 74.025201); }
        }

        protected override string Path
        {
            get { return "m 843.47738 74.025201 44.44671 -15.152288 78.7919 70.710677 5.05076 56.06347 -8.5863 4.54568 -97.47972 -25.25381 z"; }
        }
    }
}