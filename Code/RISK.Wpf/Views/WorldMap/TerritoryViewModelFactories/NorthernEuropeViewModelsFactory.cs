using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class NorthernEuropeViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public NorthernEuropeViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.NORTHERN_EUROPE; }
        }

        protected override Point NamePosition
        {
            get { return new Point(635.89103, 163.92878); }
        }

        protected override string Path
        {
            get { return "m 635.89103 163.92878 23.73858 -20.70813 48.9924 0 7.07107 43.94164 -62.62946 -3.53553 z"; }
        }
    }
}