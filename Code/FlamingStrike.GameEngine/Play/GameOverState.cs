namespace FlamingStrike.GameEngine.Play
{
    public interface IGameOverState
    {
        IPlayer Winner { get; }
    }

    public class GameOverState : IGameOverState
    {
        public GameOverState(IPlayer winner)
        {
            Winner = winner;
        }

        public IPlayer Winner { get; }
    }
}