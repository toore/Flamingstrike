using System;

namespace GuiWpf.ViewModels.Setup
{
    public interface IDispatcherWrapper
    {
        void Invoke(Action action);
    }
}