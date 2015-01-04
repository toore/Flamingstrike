using System;
using System.Windows;

namespace GuiWpf.Services
{
    public interface IGuiThreadDispatcher
    {
        void Invoke(Action action);
    }

    public class GuiThreadDispatcher : IGuiThreadDispatcher
    {
        public void Invoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}