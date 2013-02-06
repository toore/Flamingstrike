using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class KamchatkaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public KamchatkaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.KAMCHATKA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(1174.8074, 71.49982); }
        }

        protected override string Path
        {
            get { return "m 1174.8074 71.49982 108.0863 13.131983 -33.335 77.276667 -47.9822 -50.50762 -50.0026 31.31472 60.1041 43.94164 -36.3655 21.71828 -2.5254 -27.7792 -18.6878 -2.0203 -28.2843 -46.97209 z"; }
        }
    }
}