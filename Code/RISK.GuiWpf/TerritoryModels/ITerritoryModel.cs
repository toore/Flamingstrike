using System.Windows;

namespace GuiWpf.TerritoryModels
{
    public interface ITerritoryModel
    {
        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}