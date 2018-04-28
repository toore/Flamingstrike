
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;

namespace FlamingStrike.UI.WPF.ViewModels.Messages
{
    public class StartGameplayMessage
    {
        public IGamePlaySetup GamePlaySetup { get; private set; }

        public StartGameplayMessage(IGamePlaySetup gamePlaySetup)
        {
            GamePlaySetup = gamePlaySetup;
        }
    }
}