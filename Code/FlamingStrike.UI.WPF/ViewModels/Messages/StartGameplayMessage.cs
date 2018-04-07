using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;

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