using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class EasternAustraliaViewModel : TerritoryViewModelsFactoryBase
    {
        public EasternAustraliaViewModel(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.EASTERN_AUSTRALIA; }
        }
        
        protected override Point NamePosition
        {
            get { return new Point(1206.6272, 526.06847); }
        }
        
        protected override string Path
        {
            get { return "m 1206.6272 526.06847 72.2259 -8.08123 29.7995 78.28683 -79.802 61.11423 z"; }
        }
    }
}