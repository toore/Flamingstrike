namespace RISK.GameEngine.Play.GameStates
{
    public interface IGameOverGameState
    {
        IPlayer Winner { get; set; }
    }

    public class GameOverGameState : IGameOverGameState
    {
        public IPlayer Winner { get; set; }

        public GameOverGameState(IPlayer winner)
        {
            Winner = winner;
        }
    }
}