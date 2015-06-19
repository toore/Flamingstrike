namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapItemViewModelVisitor
    {
        void Visit(TerritoryViewModel territoryViewModel);
        void Visit(TitleViewModel titleViewModel);
    }
}