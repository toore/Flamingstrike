using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class SouthernEuropeViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public SouthernEuropeViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.SOUTHERN_EUROPE; }
        }

        protected override Point NamePosition
        {
            get { return new Point(653.06362, 183.62675); }
        }

        protected override string Path
        {
            get { return "m 653.06362 183.62675 -1.01015 20.70813 38.89088 26.76904 5.05076 -8.58629 -26.26397 -21.71828 12.12182 -6.06091 19.39716 17.65436 17.47342 31.8431 5.05076 -7.57614 -5.05076 -11.61675 21.71828 -20.70813 -23.73859 -17.17259 z"; }
        }
    }
}