using System.Threading.Tasks;
using Caliburn.Micro;

namespace FlamingStrike.UI.WPF.ViewModels
{
    public class NotifyViewModel : Screen
    {
        public string Message { get; set; }

        public async Task Close()
        {
            await TryCloseAsync();
        }
    }
}