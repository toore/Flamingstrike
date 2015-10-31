namespace GuiWpf.ViewModels
{
    public interface IGameOverViewModelFactory
    {
        GameOverViewModel Create(string winnerPlayerName);
    }

    public class GameOverViewModelFactory : IGameOverViewModelFactory
    {
        public GameOverViewModel Create(string winnerPlayerName)
        {
            return new GameOverViewModel(winnerPlayerName);
        }
    }
}