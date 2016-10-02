namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public interface IWorldMapItemViewModelVisitor
    {
        void Visit(RegionViewModel regionViewModel);
        void Visit(RegionNameViewModel regionNameViewModel);
    }
}