using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class WesternEuropeViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public WesternEuropeViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.WESTERN_EUROPE; }
        }

        protected override Point NamePosition
        {
            get { return new Point(636.3961, 164.93893); }
        }

        protected override string Path
        {
            get { return "m 636.3961 164.93893 -29.7995 14.14214 7.07107 26.76904 -24.74874 0 -8.08122 36.36549 33.84011 -2.02031 37.37565 -35.35533 0.50507 -22.22336 z"; }
        }
    }
}