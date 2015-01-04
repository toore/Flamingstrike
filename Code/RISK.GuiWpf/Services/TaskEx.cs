using System;
using System.Threading.Tasks;

namespace GuiWpf.Services
{
    public interface ITaskEx
    {
        Task Run(Action action);
    }

    public class TaskEx : ITaskEx
    {
        public Task Run(Action action)
        {
            return Task.Run(action);
        }
    }
}