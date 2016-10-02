using Caliburn.Micro;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.ViewModels
{
    public interface IGameOverViewModel
    {
        string PlayerNameIsTheWinnerText { get; }
    }

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