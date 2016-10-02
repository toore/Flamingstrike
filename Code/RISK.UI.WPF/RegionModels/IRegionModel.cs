using System.Windows;
using RISK.Core;

namespace RISK.UI.WPF.RegionModels
{
    public interface IRegionModel
    {
        IRegion Region { get; }

        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}