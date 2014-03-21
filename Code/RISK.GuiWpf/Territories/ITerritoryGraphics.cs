using System.Windows;

namespace GuiWpf.Territories
{
    public interface ITerritoryGraphics
    {
        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}