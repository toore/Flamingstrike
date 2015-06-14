namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModelFactory
    {
        IGameboardViewModel Create(IGameAdapter gameAdapter);
    }
}