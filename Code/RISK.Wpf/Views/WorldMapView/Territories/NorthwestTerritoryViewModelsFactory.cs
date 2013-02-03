using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class NorthwestTerritoryViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public NorthwestTerritoryViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.NORTHWEST_TERRITORY; }
        }

        protected override Point NamePosition
        {
            get { return new Point(157.07872, 72.76251); }
        }

        protected override string Path
        {
            get
            {
                return "m 157.07872 72.76251 97.22718 7.323606 67.68022 -16.667517 33.58758 11.869292 -80.05459 43.689089 -163.64472 -2.0203 z" +
                       "M 223.74879 59.377989 252.53814 45.993467 291.68155 64.68129 250.01276 75.540429 z";
            }
        }
    }
}