using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class EgyptViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public EgyptViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.EGYPT; }
        }

        protected override Point NamePosition
        {
            get { return new Point(666.1956, 255.85266); }
        }
        
        protected override string Path
        {
            get { return "m 666.1956 255.85266 105.56095 17.17259 4.54568 14.14214 -7.57614 5.05076 12.6269 24.74874 -53.033 19.69797 -64.64977 -29.7995 z"; }
        }
    }
}