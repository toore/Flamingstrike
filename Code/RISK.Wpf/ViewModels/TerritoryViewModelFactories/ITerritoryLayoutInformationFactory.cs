using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public interface ITerritoryLayoutInformationFactory
    {
        ITerritoryLayoutInformation Create(ILocation location);
    }
}