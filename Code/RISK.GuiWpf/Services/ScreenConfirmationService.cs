using Caliburn.Micro;

namespace GuiWpf.Services
{
    public interface IScreenConfirmationService
    {
        void Confirm(Screen screen);
        void Cancel(Screen screen);
    }

    public class ScreenConfirmationService : IScreenConfirmationService
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