using Caliburn.Micro;
using GuiWpf.Properties;
using RISK.Application;

namespace GuiWpf.ViewModels
{
    public class GameOverViewModel : Screen, IGameOverViewModel
    {
        public GameOverViewModel(IPlayerId winner)
        {
            PlayerNameIsTheWinnerText = string.Format(Resources.ARG0_IS_THE_WINNER, winner.Name);
        }

        public string PlayerNameIsTheWinnerText { get; private set; }

        public void Close()
        {
            TryClose();
        }
    }
}