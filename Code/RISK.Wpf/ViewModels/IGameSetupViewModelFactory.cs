using System;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(Action<GameSetup> confirm);
    }
}