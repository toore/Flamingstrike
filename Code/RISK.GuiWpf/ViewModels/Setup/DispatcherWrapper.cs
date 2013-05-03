using System;
using System.Windows.Threading;

namespace GuiWpf.ViewModels.Setup
{
    public class DispatcherWrapper : IDispatcherWrapper
    {
        public void Invoke(Action action)
        {
            Dispatcher.CurrentDispatcher.Invoke(action);
        }
    }
}