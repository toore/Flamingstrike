using RISK.GameEngine.Setup;

namespace RISK.UI.WPF.ViewModels.Messages
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