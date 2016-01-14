using System;
using GuiWpf.Services;

namespace RISK.Tests.GuiWpf
{
    public class CurrentThreadDispatcher : IGuiThreadDispatcher
    {
        public void Invoke(Action action)
        {
            action.Invoke();
        }
    }
}