namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IWorldMapItemViewModel
    {
        void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor);
    }
}