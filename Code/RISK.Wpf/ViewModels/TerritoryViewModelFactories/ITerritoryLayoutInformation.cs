using System.Windows;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public interface ITerritoryLayoutInformation
    {
        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}