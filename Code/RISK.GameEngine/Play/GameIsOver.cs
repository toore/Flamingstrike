using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IGameIsOver
    {
        IPlayer Winner { get; }
    }

    public class GameIsOver : IGameIsOver
    {
        private readonly IGameOverGameState _gameOverGameState;

        public GameIsOver(IGameOverGameState gameOverGameState)
        {
            _gameOverGameState = gameOverGameState;
        }

        public IPlayer Winner => _gameOverGameState.Winner;
    }
}