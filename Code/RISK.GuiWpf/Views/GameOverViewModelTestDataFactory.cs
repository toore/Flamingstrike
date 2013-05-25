using GuiWpf.ViewModels;
using RISK.Domain.Entities;

namespace GuiWpf.Views
{
    public class GameOverViewModelTestDataFactory
    {
        public static GameOverViewModel ViewModel
        {
            get { return new GameOverViewModelTestDataFactory().Create(); }
        }

        private GameOverViewModel Create()
        {
            return new GameOverViewModel(new HumanPlayer("TEST_PLAYER"));
        }
    }
}