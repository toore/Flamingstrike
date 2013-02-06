using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class OntarioViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public OntarioViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.ONTARIO; }
        }

        protected override Point NamePosition
        {
            get { return new Point(244.70945, 118.72445); }
        }

        protected override string Path
        {
            get { return "m 244.70945 118.72445 30.55712 0.50508 30.30458 43.43656 5.55583 34.59772 -39.39595 19.1929 -58.3363 -41.16372 z"; }
        }
    }
}