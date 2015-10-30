using System.Windows;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public interface ITerritoryModel
    {
        ITerritoryId TerritoryId { get; }

        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}