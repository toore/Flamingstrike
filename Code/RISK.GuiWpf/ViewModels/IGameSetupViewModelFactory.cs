using System;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(IGameStateConductor gameStateConductor);
    }
}