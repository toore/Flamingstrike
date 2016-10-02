using System.Windows;
using RISK.Core;

namespace RISK.UI.WPF.RegionModels
{
    public abstract class RegionModelBase : IRegionModel
    {
        protected RegionModelBase(IRegion region)
        {
            Region = region;
        }

        public IRegion Region { get; }

        public abstract string Name { get; }
        public abstract Point NamePosition { get; }
        public abstract string Path { get; }
    }
}