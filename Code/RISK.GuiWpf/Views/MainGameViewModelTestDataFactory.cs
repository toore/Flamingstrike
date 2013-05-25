using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class MainGameViewModelTestDataFactory : TestDataFactoryBase
    {
        public static IMainGameViewModel ViewModel
        {
            get { return new MainGameViewModelTestDataFactory().Create<IMainGameViewModel>(); }
        }
    }
}