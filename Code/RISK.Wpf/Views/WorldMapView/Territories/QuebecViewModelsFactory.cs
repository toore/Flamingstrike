using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class QuebecViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public QuebecViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.QUEBEC; }
        }

        protected override Point NamePosition
        {
            get { return new Point(306.5813, 163.17116); }
        }

        protected override string Path
        {
            get { return "m 306.5813 163.17116 41.16371 -57.83123 56.56855 50.00255 -92.6815 42.17387 z"; }
        }
    }
}