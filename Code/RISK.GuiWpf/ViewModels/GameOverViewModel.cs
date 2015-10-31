using Caliburn.Micro;
using GuiWpf.Properties;

namespace GuiWpf.ViewModels
{
    public interface IGameOverViewModel { }

    public class GameOverViewModel : Screen, IGameOverViewModel
    {
        public GameOverViewModel(string winnerPlayerName)
        {
            PlayerNameIsTheWinnerText = string.Format(Resources.ARG0_IS_THE_WINNER, winnerPlayerName);
        }

        public string PlayerNameIsTheWinnerText { get; private set; }

        public void Close()
        {
            TryClose();
        }
    }
}