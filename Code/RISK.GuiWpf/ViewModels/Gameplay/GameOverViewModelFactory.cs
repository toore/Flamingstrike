namespace GuiWpf.ViewModels.Gameplay
{
    public class GameOverViewModelFactory : IGameOverViewModelFactory
    {
        public GameOverViewModel Create()
        {
            return new GameOverViewModel();
        }
    }
}