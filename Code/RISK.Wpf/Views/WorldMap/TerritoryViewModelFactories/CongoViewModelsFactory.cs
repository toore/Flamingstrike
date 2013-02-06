using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class CongoViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public CongoViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.CONGO; }
        }

        protected override Point NamePosition
        {
            get { return new Point(664.1753, 433.6395); }
        }

        protected override string Path
        {
            get { return "m 664.1753 433.6395 58.58885 -48.48731 33.84011 104.55078 -34.34519 16.16244 -47.98225 -26.76903 z"; }
        }
    }
}