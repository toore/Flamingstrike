using System.Threading.Tasks;
using Caliburn.Micro;

namespace FlamingStrike.UI.WPF.Services
{
    public interface IScreenConfirmationService
    {
        Task Confirm(Screen screen);
        Task Cancel(Screen screen);
    }

    public class ScreenConfirmationService : IScreenConfirmationService
    {
        public async Task Confirm(Screen screen)
        {
            await screen.TryCloseAsync(true);
        }

        public async Task Cancel(Screen screen)
        {
            await screen.TryCloseAsync(false);
        }
    }
}