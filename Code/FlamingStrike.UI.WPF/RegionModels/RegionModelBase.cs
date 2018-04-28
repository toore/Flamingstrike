using System.Windows;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public abstract class RegionModelBase : IRegionModel
    {
        protected RegionModelBase(Region region)
        {
            Region = region;
        }

        public Region Region { get; }

        public abstract string Name { get; }
        public abstract Point NamePosition { get; }
        public abstract string Path { get; }
    }
}