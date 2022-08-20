using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace FlamingStrike.UI.WPF.Infrastructure
{
    public class WindowManager : Caliburn.Micro.WindowManager
    {
        protected override async Task<Window> CreateWindowAsync(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            var window = await base.CreateWindowAsync(rootModel, isDialog, context, settings);

            if (isDialog)
            {
                window.Visibility = Visibility.Collapsed;
            }

            if (!isDialog)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.SizeToContent = SizeToContent.Manual;
                window.Width = 1366;
                window.Height = 768;
            }

            return window;
        }
    }
}