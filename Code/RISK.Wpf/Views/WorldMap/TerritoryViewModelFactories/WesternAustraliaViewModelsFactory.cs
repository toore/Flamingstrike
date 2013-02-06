using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class WesternAustraliaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public WesternAustraliaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.WESTERN_AUSTRALIA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(1207.6374, 526.06847); }
        }

        protected override string Path
        {
            get { return "m 1207.6374 526.06847 -74.7513 40.91117 -10.1015 68.69038 106.571 20.70812 z"; }
        }
    }
}