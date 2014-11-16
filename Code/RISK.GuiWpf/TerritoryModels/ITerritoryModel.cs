using System.Windows;
using RISK.Application.Entities;

namespace GuiWpf.TerritoryModels
{
    public interface ITerritoryModel
    {
        ITerritory Territory { get; }

        string Name { get; }
        Point NamePosition { get; }
        string Path { get; }
    }
}