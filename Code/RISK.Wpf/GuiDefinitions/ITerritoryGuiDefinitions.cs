using System.Windows;

namespace GuiWpf.GuiDefinitions
{
    public interface ITerritoryGuiDefinitions
    {
        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}