using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class MongoliaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public MongoliaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.MONGOLIA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(1110.6627, 214.4364); }
        }

        protected override string Path
        {
            get { return "m 1110.6627 214.4364 67.6802 41.92134 -5.5558 -75.76145 -184.35283 -16.66751 -9.09138 11.61675 69.70051 51.51778 z"; }
        }
    }
}