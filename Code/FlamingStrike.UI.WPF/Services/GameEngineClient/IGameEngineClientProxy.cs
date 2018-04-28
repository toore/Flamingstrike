using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public interface IGameEngineClientProxy
    {
        void Setup(IAlternateGameSetupObserver alternateGameSetupObserver, IEnumerable<string> players);
        void StartGame(IGameplayViewModel gameplayViewModel, IGamePlaySetup gamePlaySetup);
    }
}