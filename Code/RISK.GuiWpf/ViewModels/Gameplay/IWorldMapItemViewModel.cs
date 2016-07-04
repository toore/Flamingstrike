namespace GuiWpf.ViewModels.Gameplay
{
    public interface IWorldMapItemViewModel
    {
        void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor);
    }
}