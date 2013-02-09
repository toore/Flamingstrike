using RISK.Domain.Entities;

namespace GuiWpf.GuiDefinitions
{
    public interface ITerritoryGuiDefinitionFactory
    {
        ITerritoryGuiDefinitions Create(ILocation location);
    }
}