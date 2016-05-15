using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Messages
{
    public class StartGameplayMessage
    {
        public IGame Game { get; private set; }

        public StartGameplayMessage(IGame game)
        {
            Game = game;
        }
    }
}