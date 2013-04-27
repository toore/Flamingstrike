using System.Windows;

namespace GuiWpf.Territories
{
    public interface ITerritoryGui
    {
        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}