using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public interface IGameEngineClientProxy
    {
        void Setup(IAlternateGameSetupObserver alternateGameSetupObserver, IEnumerable<string> players);
        void StartGame(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup);
    }
}