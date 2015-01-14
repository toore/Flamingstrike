namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapItemViewModelVisitor
    {
        void Visit(TerritoryLayoutViewModel territoryLayoutViewModel);
        void Visit(TerritoryTextViewModel territoryTextViewModel);
    }
}