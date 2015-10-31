using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class GameOverViewModelTestDataFactory
    {
        public static GameOverViewModel ViewModel => new GameOverViewModelTestDataFactory().Create();

        private GameOverViewModel Create()
        {
            return new GameOverViewModel("TEST_PLAYER");
        }
    }
}