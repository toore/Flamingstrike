namespace GuiWpf.ViewModels
{
    public class GameOverViewModelFactory : IGameOverViewModelFactory
    {
        public GameOverViewModel Create()
        {
            return new GameOverViewModel();
        }
    }
}