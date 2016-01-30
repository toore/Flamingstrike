using System.Windows;
using RISK.Application.World;

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