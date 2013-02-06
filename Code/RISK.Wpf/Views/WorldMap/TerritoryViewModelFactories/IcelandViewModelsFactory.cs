using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class IcelandViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public IcelandViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.ICELAND; }
        }

        protected override Point NamePosition
        {
            get { return new Point(546.9976, 90.692718); }
        }

        protected override string Path
        {
            get { return "M 546.9976 90.692718 552.55344 107.36023 585.3834 94.733328 575.28187 82.106421 z"; }
        }
    }
}