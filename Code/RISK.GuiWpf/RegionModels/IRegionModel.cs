using System.Windows;
using RISK.Core;

namespace GuiWpf.TerritoryModels
{
    public interface IRegionModel
    {
        IRegion Region { get; }

        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}