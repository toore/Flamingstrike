namespace FlamingStrike.GameEngine.Play
{
    public interface IGameOverState
    {
        PlayerName Winner { get; }
    }

    public class GameOverState : IGameOverState
    {
        public GameOverState(PlayerName winner)
        {
            Winner = winner;
        }

        public PlayerName Winner { get; }
    }
}