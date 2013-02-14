using System;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        public IGameSetupViewModel Create(Action<GameSetup> confirm)
        {
            return new GameSetupViewModel(confirm);
        }
    }
}