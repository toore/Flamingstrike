using Caliburn.Micro;
using GuiWpf.Properties;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels
{
    public class GameOverViewModel : Screen, IGameOverViewModel
    {
        private readonly IPlayer _winner;

        public GameOverViewModel(IPlayer winner)
        {
            _winner = winner;

            PlayerNameIsTheWinnerText = string.Format(Resources.ARG0_IS_THE_WINNER, winner.Name);
        }

        public IPlayer WinnerPlayer { get; private set; }

        public string PlayerNameIsTheWinnerText { get; private set; }
    }
}