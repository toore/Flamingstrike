using GuiWpf.ViewModels;
using RISK.Application;

namespace GuiWpf.Views
{
    public class GameOverViewModelTestDataFactory
    {
        public static GameOverViewModel ViewModel => new GameOverViewModelTestDataFactory().Create();

        private GameOverViewModel Create()
        {
            return new GameOverViewModel(new Player("TEST_PLAYER"));
        }
    }
}