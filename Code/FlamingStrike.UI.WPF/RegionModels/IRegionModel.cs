using System.Windows;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public interface IRegionModel
    {
        Region Region { get; }
        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}