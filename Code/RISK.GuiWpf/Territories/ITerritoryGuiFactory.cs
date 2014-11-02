using RISK.Application.Entities;

namespace GuiWpf.Territories
{
    public interface ITerritoryGuiFactory
    {
        ITerritoryGraphics Create(ITerritory location);
    }
}