using RISK.Domain.Entities;

namespace GuiWpf.Services
{
    public interface ITerritoryColorsFactory 
    {
        ITerritoryColors Create(ITerritory territory);
    }
}