namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public interface IGameOverState
    {
        string Winner { get; }
    }

    public class GameOverState : IGameOverState
    {
        public GameOverState(string winner)
        {
            Winner = winner;
        }

        public string Winner { get; }
    }
}