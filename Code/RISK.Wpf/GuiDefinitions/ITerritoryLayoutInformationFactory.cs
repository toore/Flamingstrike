using RISK.Domain.Entities;

namespace GuiWpf.GuiDefinitions
{
    public interface ITerritoryLayoutInformationFactory
    {
        ITerritoryGuiDefinitions Create(ILocation location);
    }
}