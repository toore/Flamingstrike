using System.Collections.Generic;
using System.Windows;

namespace RISK.UI.WPF.Infrastructure
{
    public class WindowManager : Caliburn.Micro.WindowManager
    {
        protected override Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            var window = base.CreateWindow(rootModel, isDialog, context, settings);

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