using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.Messages
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