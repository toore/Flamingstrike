using System.Windows;
using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public interface RegionModel
    {
        Region Region { get; }

        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}