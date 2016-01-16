using System.Windows;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public interface ITerritoryModel
    {
        ITerritoryGeography TerritoryGeography { get; }

        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}