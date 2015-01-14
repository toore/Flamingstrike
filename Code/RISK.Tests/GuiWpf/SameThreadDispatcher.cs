using System;
using GuiWpf.Services;

namespace RISK.Tests.GuiWpf
{
    public class SameThreadDispatcher : IGuiThreadDispatcher
    {
        public void Invoke(Action action)
        {
            action.Invoke();
        }
    }
}