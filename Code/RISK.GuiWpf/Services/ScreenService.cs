using Caliburn.Micro;

namespace GuiWpf.Services
{
    public interface IScreenService
    {
        void Confirm(Screen screen);
        void Cancel(Screen screen);
    }

    public class ScreenService : IScreenService
    {
        public void Confirm(Screen screen)
        {
            screen.TryClose(true);
        }

        public void Cancel(Screen screen)
        {
            screen.TryClose(false);
        }
    }
}