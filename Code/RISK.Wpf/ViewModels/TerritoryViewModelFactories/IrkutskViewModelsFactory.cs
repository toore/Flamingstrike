using System.Windows;
using GuiWpf.Properties;
using GuiWpf.Services;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public class IrkutskViewModelsFactory : TerritoryViewModelsFactoryBase
    {
        public IrkutskViewModelsFactory(ITerritoryColors territoryColors) : base(territoryColors) {}

        protected override string Name
        {
            get { return Resources.IRKUTSK; }
        }
        
        protected override Point NamePosition
        {
            get { return new Point(1012.1729, 108.37039); }
        }
        
        protected override string Path
        {
            get { return "m 1012.1729 108.37039 113.6421 23.73858 27.7792 45.96194 -165.66501 -14.64721 31.81981 -40.4061 z"; }
        }
    }
}