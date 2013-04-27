using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class MainGameViewTestDataFactory : TestDataFactoryBase
    {
        public static IMainGameViewModel ViewModel
        {
            get { return new MainGameViewTestDataFactory().Create<IMainGameViewModel>(); }
        }
    }
}