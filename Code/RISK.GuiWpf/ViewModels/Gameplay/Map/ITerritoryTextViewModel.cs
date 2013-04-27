using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryTextViewModel : IWorldMapViewModel
    {
        ILocation Location { get; }
        int Armies { get; set; }
    }
}