using Caliburn.Micro;
using GuiWpf.Properties;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels
{
    public class GameOverViewModel : Screen, IGameOverViewModel
    {
        public GameOverViewModel(IPlayer winner)
        {
            PlayerNameIsTheWinnerText = string.Format(Resources.ARG0_IS_THE_WINNER, winner.Name);
        }

        public string PlayerNameIsTheWinnerText { get; private set; }
    }
}