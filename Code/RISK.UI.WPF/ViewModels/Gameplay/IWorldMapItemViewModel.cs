namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public interface IWorldMapItemViewModel
    {
        void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor);
    }
}