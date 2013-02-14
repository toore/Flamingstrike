using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class MainGameViewTestDataFactory
    {
        private IMainGameViewModel Create()
        {
            return new MainGameViewModel(new GameSetupViewModelFactory(null), null, null);
        }

        public static IMainGameViewModel ViewModel
        {
            get { return new MainGameViewTestDataFactory().Create(); }
        }
    }
}