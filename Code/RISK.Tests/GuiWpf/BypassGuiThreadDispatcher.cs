using System;
using GuiWpf.Services;

namespace RISK.Tests.GuiWpf
{
    public class BypassGuiThreadDispatcher : IGuiThreadDispatcher
    {
        public void Invoke(Action action)
        {
            action.Invoke();
        }
    }
}