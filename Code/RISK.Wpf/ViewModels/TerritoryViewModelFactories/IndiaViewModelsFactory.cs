using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class IndiaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public IndiaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.INDIA; }
        }
        
        protected override Point NamePosition
        {
            get { return new Point(889.94439, 249.28667); }
        }
        
        protected override string Path
        {
            get { return "m 889.94439 249.28667 36.36549 -28.78935 126.26912 73.74114 -81.31732 110.6117 -75.76145 -98.99495 z"; }
        }
    }
}