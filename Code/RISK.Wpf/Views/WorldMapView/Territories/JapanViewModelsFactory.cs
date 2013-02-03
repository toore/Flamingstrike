using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class JapanViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public JapanViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.JAPAN; }
        }

        protected override Point NamePosition
        {
            get { return new Point(1208.6475, 196.25366); }
        }

        protected override string Path
        {
            get { return "m 1208.6475 196.25366 11.1117 41.92133 -28.2843 20.70813 7.0711 16.66751 36.8706 -27.27411 -6.061 -45.96195 z"; }
        }
    }
}