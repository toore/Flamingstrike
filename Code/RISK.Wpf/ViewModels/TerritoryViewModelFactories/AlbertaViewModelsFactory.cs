using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class AlbertaViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public AlbertaViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.ALBERTA; }
        }

        protected override Point NamePosition
        {
            get { return new Point(80.76399, 175.79807); }
        }

        protected override string Path
        {
            get { return "m 125.76399 175.79807 87.12566 0.25254 31.8198 -57.07362 -132.07745 -2.0203 5.70411 11.64002 z"; }
        }
    }
}