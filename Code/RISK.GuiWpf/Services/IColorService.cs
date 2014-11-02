using System.Windows.Media;
using RISK.Application.Entities;

namespace GuiWpf.Services
{
    public interface IColorService
    {
        ITerritoryColors NorthAmericaColors { get; }
        ITerritoryColors SouthAmericaColors { get; }
        ITerritoryColors EuropeColors { get; }
        ITerritoryColors AfricaColors { get; }
        ITerritoryColors AsiaColors { get; }
        ITerritoryColors AustraliaColors { get; }
        
        ITerritoryColors GetPlayerTerritoryColors(IPlayer player);

        Color SelectedTerritoryColor { get; }
    }
}