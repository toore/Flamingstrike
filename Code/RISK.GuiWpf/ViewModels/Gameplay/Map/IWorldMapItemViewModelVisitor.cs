namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapItemViewModelVisitor
    {
        void Visit(RegionViewModel regionViewModel);
        void Visit(RegionNameViewModel regionNameViewModel);
    }
}