namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapItemViewModel
    {
        void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor);
    }
}