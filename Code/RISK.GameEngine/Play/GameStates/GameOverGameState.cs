namespace RISK.GameEngine.Play.GameStates
{
    public interface IGameOverGameState {}

    public class GameOverGameState : IGameOverGameState
    {
        private readonly IPlayer _winner;

        public GameOverGameState(IPlayer winner)
        {
            _winner = winner;
        }
    }
}