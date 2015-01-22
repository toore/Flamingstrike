using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels.Messages
{
    public class StartGameplayMessage
    {
        public IGameAdapter GameAdapter { get; private set; }

        public StartGameplayMessage(IGameAdapter gameAdapter)
        {
            GameAdapter = gameAdapter;
        }
    }
}