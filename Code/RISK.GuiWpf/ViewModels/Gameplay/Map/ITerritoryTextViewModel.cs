using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryTextViewModel : IWorldMapItemViewModel
    {
        ITerritory Territory { get; }
        int Armies { get; set; }
    }
}