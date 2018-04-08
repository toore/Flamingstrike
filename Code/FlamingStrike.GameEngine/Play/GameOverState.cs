namespace FlamingStrike.GameEngine.Play
{
    public class GameOverState : IGameOverState
    {
        public GameOverState(PlayerName winner)
        {
            Winner = winner;
        }

        public PlayerName Winner { get; }
    }
}