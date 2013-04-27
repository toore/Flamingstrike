using RISK.Domain.Entities;

namespace GuiWpf.Territories
{
    public interface ITerritoryGuiFactory
    {
        ITerritoryGui Create(ILocation location);
    }
}