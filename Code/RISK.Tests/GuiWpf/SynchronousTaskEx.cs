using System;
using System.Threading.Tasks;
using GuiWpf.Services;

namespace RISK.Tests.GuiWpf
{
    public class SynchronousTaskEx : ITaskEx
    {
        public Task Run(Action action)
        {
            var task = new Task(action);
            task.RunSynchronously();
            return task;
        }
    }
}