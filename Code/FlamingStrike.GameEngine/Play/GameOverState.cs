using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGameOverState
    {
        IPlayer Winner { get; }
    }

    public class GameOverState : IGameOverState
    {
        private readonly IGameOverGameState _gameOverGameState;

        public GameOverState(IGameOverGameState gameOverGameState)
        {
            _gameOverGameState = gameOverGameState;
        }

        public IPlayer Winner => _gameOverGameState.Winner;
    }
}