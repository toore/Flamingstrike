using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class AfhanistanViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public AfhanistanViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.AFGHANISTAN; }
        }

        protected override Point NamePosition
        {
            get { return new Point(802.06112, 212.92118); }
        }

        protected override string Path
        {
            get { return "m 802.06112 212.92118 64.14469 -48.48733 96.97464 26.26397 -72.73098 58.08377 z"; }
        }
    }
}