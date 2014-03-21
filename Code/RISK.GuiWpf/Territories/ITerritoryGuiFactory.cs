using RISK.Domain.Entities;

namespace GuiWpf.Territories
{
    public interface ITerritoryGuiFactory
    {
        ITerritoryGraphics Create(ILocation location);
    }
}