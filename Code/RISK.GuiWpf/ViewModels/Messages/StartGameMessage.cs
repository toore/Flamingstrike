using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels.Messages
{
    public class StartGameMessage
    {
        public IGame Game { get; private set; }

        public StartGameMessage(IGame game)
        {
            Game = game;
        }
    }
}