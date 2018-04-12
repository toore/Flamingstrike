using System.Windows;
using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public abstract class RegionModelBase : RegionModel
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