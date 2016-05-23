namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapItemViewModelVisitor
    {
        void Visit(RegionOutlineViewModel regionViewModel);
        void Visit(RegionNameViewModel regionNameViewModel);
    }
}